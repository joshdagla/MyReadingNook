using MyReadingNookApp.Dtos;
using MyReadingNookApp.Services;
using Xunit;

namespace MyReadingNookApp.Tests;

public class FriendInviteServiceTests
{
    [Fact]
    public async Task SendFriendInviteAsync_SelfRequest_ReturnsFailure()
    {
        using var context = TestNookContextFactory.Create();
    }
}
