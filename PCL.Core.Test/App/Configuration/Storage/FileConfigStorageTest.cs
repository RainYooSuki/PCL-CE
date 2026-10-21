using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCL.Core.App.Configuration.Storage;

namespace PCL.Core.Test.App.Configuration.Storage;

[TestClass]
public class FileConfigStorageTest
{
    [TestMethod]
    public void Stop_ShouldPersistQueuedWrite()
    {
        var provider = new TestFileProvider();
        var storage = new FileConfigStorage(provider);

        storage.SetValue("JvmArgs", "-Xmx2G");
        storage.Stop();

        Assert.AreEqual("-Xmx2G", provider.Get<string>("JvmArgs"));
    }

    private sealed class TestFileProvider : IKeyValueFileProvider
    {
        private readonly Dictionary<string, object?> _values = [];

        public string FilePath => "test://config";

        public T Get<T>(string key) =>
            _values[key] is T value
                ? value
                : throw new InvalidOperationException($"Value for '{key}' is null or not of type {typeof(T)}.");

        public void Set<T>(string key, T value) => _values[key] = value;

        public bool Exists(string key) => _values.ContainsKey(key);

        public void Remove(string key) => _values.Remove(key);

        public void Sync() { }
    }
}
