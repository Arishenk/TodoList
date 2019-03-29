namespace TodoItems.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TodoItems.API.Errors;
    using TodoItems.Models.Converters.TodoItems;
    using TodoItems.Models.TodoItems.Repositories;
    using Model = global::TodoItems.Models;
    using global::TokenApp.Controllers;
    using global::TokenApp;
    using TodoItems.Models.Users;
    using System.Data.SQLite;
    using System.Data.Common;
    using TodoItems.Client.TodoItems;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using TodoItems.Models.Users.Repositories;

    [Authorize]
    [Route("v1/items")]
    public sealed class TodoItemsController : Controller
    {
        private readonly  DatabaseItemRepository itemsRepository;
        private readonly  DatabaseUserRepository usersRepository;

        public TodoItemsController(DatabaseItemRepository itemsRepository, DatabaseUserRepository usersRepository)
        {
            this.itemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
            this.usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }
              
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateItemsAsync([FromBody]Client.TodoItems.TodoItemBuildInfo buildInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (buildInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("TodoItemBuildInfo");
                return this.BadRequest(error);
            }

            var name = User.Identity.Name;
            var usser = this.usersRepository.Get(name).Id;

            var creationInfo = TodoItemBuildInfoConverter.Convert(usser.ToString(), buildInfo);

            var modelItemInfo = await this.itemsRepository.CreateAsync(creationInfo, cancellationToken).ConfigureAwait(false);

            var clientItemInfo = TodoItemInfoConverter.Convert(modelItemInfo);

            var routeParams = new Dictionary<string, object>
            {
                { "itemId", clientItemInfo.Id }
            };

            return this.CreatedAtRoute("GetItemRoute", routeParams, clientItemInfo);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchItemsAsync([FromQuery]Client.TodoItems.TodoItemInfoSearchQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelQuery = TodoItemInfoSearchQueryConverter.Convert(query ?? new Client.TodoItems.TodoItemInfoSearchQuery());
            var userid = this.usersRepository.Get(User.Identity.Name).Id;
            var modelItems = await this.itemsRepository.SearchAsync(modelQuery, cancellationToken, userid).ConfigureAwait(false);
            var clientItems = modelItems.Select(item => TodoItemInfoConverter.Convert(item)).ToList();
            var clientItemsList = new Client.TodoItems.ItemsList
            {
                TodoItems = clientItems
            };

            return this.Ok(clientItemsList);
        }

        [HttpGet]
        [Route("{itemId}", Name = "GetItemRoute")]
        public IActionResult GetItem([FromRoute]string itemId, CancellationToken cancelltionToken)
        {
            cancelltionToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(itemId, out var modelItemId))
            {
                var error = ServiceErrorResponses.ItemNotFound(itemId);
                return this.NotFound(error);
            }

            Model.TodoItems.TodoItem modelItem = null;
            var userid = this.usersRepository.Get(User.Identity.Name).Id;

            try
            {
                modelItem = this.itemsRepository.GetAsync(modelItemId, cancelltionToken, userid);
            }
            catch (Model.TodoItems.TodoItemNotFoundExcepction)
            {
                var error = ServiceErrorResponses.ItemNotFound(itemId);
                return this.NotFound(error);
            }

            var clientItem = TodoItemConverter.Convert(modelItem);

            return this.Ok(clientItem);
        }

        [HttpPatch]
        [Route("{itemId}")]
        public async Task<IActionResult> PatchNoteAsync([FromRoute]string itemId, [FromBody]Client.TodoItems.TodoItemPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("ItemPatchInfo");
                return this.BadRequest(error);
            }

            if (!Guid.TryParse(itemId, out var ItemIdGuid))
            {
                var error = ServiceErrorResponses.ItemNotFound(itemId);
                return this.NotFound(error);
            }

            var modelPathInfo = TodoItemPathcInfoConverter.Convert(ItemIdGuid, patchInfo);

            Model.TodoItems.TodoItem modelItem = null;
            var userid = this.usersRepository.Get(User.Identity.Name).Id;

            try
            {
                modelItem = await this.itemsRepository.PatchAsync(modelPathInfo, cancellationToken, userid).ConfigureAwait(false);
            }
            catch (Model.TodoItems.TodoItemNotFoundExcepction)
            {
                var error = ServiceErrorResponses.ItemNotFound(itemId);
                return this.NotFound(error);
            }

            var clientItem = TodoItemConverter.Convert(modelItem);
            return this.Ok(clientItem);
        }

        [HttpDelete]
        [Route("{itemId}")]
        public async Task<IActionResult> DeleteItemAsync([FromRoute]string itemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(itemId, out var itemIdGuid))
            {
                var error = ServiceErrorResponses.ItemNotFound(itemId);
                return this.NotFound(error);
            }
            var userid = this.usersRepository.Get(User.Identity.Name).Id;

            try
            {
                await this.itemsRepository.RemoveAsync(itemIdGuid, cancellationToken, userid).ConfigureAwait(false);
            }
            catch (Model.TodoItems.TodoItemNotFoundExcepction)
            {
                var error = ServiceErrorResponses.ItemNotFound(itemId);
                return this.NotFound(error);
            }

            return this.NoContent();
        }    
    }
}

