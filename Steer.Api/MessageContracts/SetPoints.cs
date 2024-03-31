﻿using MongoDB.Bson;

namespace Steer.Api.MessageContracts
{
    public class SetPoints
    {
        public ObjectId UserId { get; set; }
        public ObjectId ClanId { get; set; }
        public int Points { get; set; }
    }
}