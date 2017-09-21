//using DotNetCore.Core.Domain.Accounts;
//using DotNetCore.Core.Events;
//using DotNetCore.Service.Events;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace DotNetCore.Service
//{
//    /// <summary>
//    /// event comsumer
//    /// </summary>
//    public class EventConsumer : IConsumer<EntityInserted<UserInfo>>,
//        IConsumer<EntityDeleted<UserInfo>>,
//        IConsumer<EntityUpdated<UserInfo>>
//    {
        
//        private readonly ILogger _logger;
//        public EventConsumer(ILogger<EventConsumer> logger)
//        {
//            _logger = logger;
//        }
//        public void HandleEvent(EntityInserted<UserInfo> eventMessage)
//        {
//            _logger.LogInformation("UserInfo has been inserted!");
//        }

//        public void HandleEvent(EntityDeleted<UserInfo> eventMessage)
//        {
//            _logger.LogInformation("UserInfo has been deleted!");
//        }

//        public void HandleEvent(EntityUpdated<UserInfo> eventMessage)
//        {
//            _logger.LogInformation("UserInfo has been updated!");
//        }
//    }
//}
