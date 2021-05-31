namespace Hexalith.WorkItems.Infrastructure.DevOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.Services.Common;
    using Microsoft.VisualStudio.Services.Graph;
    using Microsoft.VisualStudio.Services.Graph.Client;

    internal class DevOpsGroup
    {
        private readonly string _groupName;
        private readonly DevOpsServer _server;
        private GraphHttpClient? _collectionClient;
        private Dictionary<string, SubjectDescriptor>? _descriptors;
        private GraphGroup? _graphGroup;

        public DevOpsGroup(DevOpsServer server, string groupName)
        {
            _server = server;
            _groupName = groupName;
        }

        public async Task<GraphGroup> GetGroup(CancellationToken cancellationToken = default)
        {
            if (_graphGroup == null)
            {
                var graphClient = await GetGraphClient(cancellationToken);
                SubjectDescriptor descriptor = (await GetGroupDescriptors(cancellationToken))
                    .Where(p => string.Equals(_groupName, p.Key, StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.Value)
                    .FirstOrDefault();
                if (string.IsNullOrWhiteSpace(descriptor.Identifier))
                {
                    throw new DevOpsGroupNotFoundException(_groupName, (await GetGroupDescriptors(cancellationToken)).Keys);
                }
                _graphGroup = await graphClient.GetGroupAsync(descriptor, null, cancellationToken);
            }
            return _graphGroup;
        }

        public async Task<Dictionary<string, SubjectDescriptor>> GetGroupDescriptors(CancellationToken cancellationToken = default)
        {
            if (_descriptors == null)
            {
                var graphClient = await GetGraphClient(cancellationToken);
                var groups = await graphClient.ListGroupsAsync(null, null, null, null, cancellationToken);
                var descriptors = groups.GraphGroups.ToDictionary(k => k.PrincipalName, v => v.Descriptor);
                if (groups.ContinuationToken != null)
                {
                    var continuationTasks = groups
                            .ContinuationToken
                            .Select(p => graphClient
                                .ListGroupsAsync(null, null, p, null, cancellationToken));
                    foreach (GraphGroup group in (await Task.WhenAll(continuationTasks)).SelectMany(p => p.GraphGroups))
                    {
                        descriptors.Add(group.PrincipalName, group.Descriptor);
                    }
                }
                _descriptors = descriptors;
            }
            return _descriptors;
        }

        public async Task<IEnumerable<GraphUser>> GetMembers(CancellationToken cancellationToken = default)
            => await GetMembers(new[] { (await GetGroup(cancellationToken)).Descriptor }, new HashSet<SubjectDescriptor>(), cancellationToken);

        private async Task<GraphHttpClient> GetGraphClient(CancellationToken cancellationToken = default)
            => _collectionClient ??= await _server.Connection.GetClientAsync<GraphHttpClient>(cancellationToken);

        private async Task<IEnumerable<GraphUser>> GetMembers(IEnumerable<SubjectDescriptor> parents, HashSet<SubjectDescriptor> found, CancellationToken cancellationToken = default)
        {
            found.AddRange(parents);
            var graphClient = await GetGraphClient(cancellationToken);
            var membersTasks = parents
                .Select(p => graphClient.ListMembershipsAsync(
                    p,
                    GraphTraversalDirection.Down,
                    1,
                    null,
                    cancellationToken))
                .ToList();
            var members = (await Task.WhenAll(membersTasks))
                .SelectMany(p => p)
                .Select(p => p.MemberDescriptor)
                .ToList();
            var noDupMembers = members
                .Where(p => !found.Contains(p))
                .ToList();
            var usersTasks = noDupMembers
                .Where(p => p.SubjectType == "aad" || p.SubjectType == "ad" || p.SubjectType == "msa")
                .Select(p => graphClient.GetUserAsync(p, cancellationToken))
                .ToList();
            var users = new List<GraphUser>(await Task.WhenAll(usersTasks));
            found.AddRange(users.ConvertAll(p => p.Descriptor));
            var groups = noDupMembers
                .Where(p => p.SubjectType == "aadgp" || p.SubjectType == "vssgp")
                .ToList();
            if (groups.Count > 0)
            {
                var subUsers = await GetMembers(groups, found, cancellationToken);
                if (subUsers.Any())
                {
                    users.AddRange(subUsers);
                }
            }
            return users.ToList();
        }
    }
}