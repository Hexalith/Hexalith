using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

using Hexalith.Application.Repositories;
using Hexalith.Infrastructure.EfCore.Helpers;

namespace Hexalith.Infrastructure.EfCore.Repositories
{
    public class State : IRepositoryDbSet, IRepositoryStateMetadata
    {
        private string _id = string.Empty;
        private int _idHash;

        public string CreatedByUser { get; set; } = string.Empty;

        public DateTime CreatedUtcDateTime { get; set; }

        public string Id
        {
            get => _id;
            set
            {
                _id = value.Trim();
                _idHash = _id.HashKey();
            }
        }

        public int IdHash
        {
            get => _idHash;
        }

        public string? Json { get; set; }
        public string? JsonType { get; set; }

        public string? LastModifiedByUser { get; set; }

        public DateTime? LastModifiedUtcDateTime { get; set; }

        [NotMapped]
        public object? Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Json))
                {
                    return null;
                }
                var type = Type.GetType(JsonType ?? string.Empty);
                if (type == null)
                {
                    throw new TypeInitializationException(JsonType, null);
                }
                return JsonSerializer.Deserialize(Json, type);
            }
            set
            {
                if (value == null)
                {
                    Json = JsonType = null;
                }
                else
                {
                    JsonType = value.GetType().AssemblyQualifiedName;
                    Json = JsonSerializer.Serialize(value);
                }
            }
        }

        public int Version { get; }
    }
}