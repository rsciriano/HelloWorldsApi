using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Aggregates.Worlds;

namespace IntegrationTests.Utils;

internal class TestDataManager
{
    private readonly Fixture _builders;
    private readonly IWorldRepository _worldRepository;
    private Random _random = new Random(); 

    public TestDataManager(Fixture builders,IWorldRepository worldRepository)
    {
        _builders = builders ?? throw new ArgumentNullException(nameof(builders));
        _worldRepository = worldRepository ?? throw new ArgumentNullException(nameof(worldRepository));
    }

    public async Task<World> CreateAndSaveNewWorld()
    {
        var newWorld = _builders
            .Build<World>()
                .With(x => x.Name, GetRandomName())
            .Create();
        await _worldRepository.Create(newWorld);
        return newWorld;
    }

    string GetRandomName()
    {
        int length = _random.Next(3, 12);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
