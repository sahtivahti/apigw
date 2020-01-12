using System;
using System.Threading.Tasks;
using apigw.Cache;
using EasyCaching.Core;
using Moq;
using Xunit;

namespace apigw.Test.Integration.Cache
{
    public class SimpleCacheTest
    {
        private readonly Mock<IEasyCachingProviderFactory> _cacheProviderFactoryMock;
        private readonly Mock<IEasyCachingProvider> _cacheProviderMock;

        public SimpleCacheTest()
        {
            _cacheProviderFactoryMock = new Mock<IEasyCachingProviderFactory>();
            _cacheProviderMock = new Mock<IEasyCachingProvider>();

            _cacheProviderFactoryMock.Setup(_ => _.GetCachingProvider(It.IsAny<string>()))
                .Returns(_cacheProviderMock.Object);
        }

        [Fact]
        public async void GetByKey_ReturnsCachedValueIfExists()
        {
            var cache = new SimpleCache<string>(_cacheProviderFactoryMock.Object);
            _cacheProviderMock.Setup(_ => _.GetAsync<string>(It.IsAny<string>()))
                .Returns(Task.FromResult(new EasyCaching.Core.CacheValue<string>("foo", true)));

            var result = await cache.Get("mycachekey", _ =>
            {
                return Task.FromResult("bar");
            });

            Assert.Equal("foo", result.Value);
        }

        [Fact]
        public async void GetByKey_ReturnsValueOfCallbackIfDoesNotExist()
        {
            var cache = new SimpleCache<string>(_cacheProviderFactoryMock.Object);
            _cacheProviderMock.Setup(_ => _.GetAsync<string>(It.IsAny<string>()))
                .Returns(Task.FromResult(new EasyCaching.Core.CacheValue<string>(null, false)));

            var result = await cache.Get("mycachekey", _ =>
            {
                return Task.FromResult("bar");
            });

            Assert.Equal("bar", result.Value);
        }

        [Fact]
        public async void TheCallback_WillSetKeyWithValueToCacheProvider()
        {
            var cache = new SimpleCache<string>(_cacheProviderFactoryMock.Object);
            _cacheProviderMock.Setup(_ => _.GetAsync<string>(It.IsAny<string>()))
                .Returns(Task.FromResult(new EasyCaching.Core.CacheValue<string>(null, false)));

            var result = await cache.Get("mycachekey", _ =>
            {
                return Task.FromResult("bar");
            });

            _cacheProviderMock.Verify(_ => _.TrySetAsync<string>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<TimeSpan>()
            ), Times.Once());

            Assert.Equal("bar", result.Value);
        }

        [Fact]
        public async void ClearKey_CallsCacheProvider()
        {
            var cache = new SimpleCache<string>(_cacheProviderFactoryMock.Object);

            await cache.Clear("foo");

            _cacheProviderMock.Verify(_ => _.RemoveAsync(It.IsAny<string>()), Times.Once());
        }
    }
}
