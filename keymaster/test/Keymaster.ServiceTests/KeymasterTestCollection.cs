// <copyright file="KeymasterTestCollection.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Associates the UsersTestFixture with an xUnit Collection
// </summary>

namespace Keymaster.ServiceTests
{
    using Xunit;

    [CollectionDefinition(nameof(KeymasterTestCollection))]
    public class KeymasterTestCollection : ICollectionFixture<KeymasterTestFixture>
    {
        // Intentionally left blank
    }
}
