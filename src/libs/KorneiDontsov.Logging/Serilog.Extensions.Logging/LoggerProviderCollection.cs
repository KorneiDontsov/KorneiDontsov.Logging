﻿// ReSharper disable All

#nullable disable

// Copied from
// https://github.com/serilog/serilog-extensions-logging/blob/v3.0.1/src/Serilog.Extensions.Logging/Extensions/Logging/LoggerProviderCollection.cs

// Copyright 2019 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Serilog.Extensions.Logging
{
    /// <summary>
    /// A dynamically-modifiable collection of <see cref="ILoggerProvider"/>s.
    /// </summary>
    class LoggerProviderCollection : IDisposable
    {
        volatile ILoggerProvider[] _providers = new ILoggerProvider[0];

        /// <summary>
        /// Add <paramref name="provider"/> to the collection.
        /// </summary>
        /// <param name="provider">A logger provider.</param>
        public void AddProvider(ILoggerProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var existing = _providers;
            var added = existing.Concat(new[] {provider}).ToArray();

#pragma warning disable 420 // ref to a volatile field
            while (Interlocked.CompareExchange(ref _providers, added, existing) != existing)
#pragma warning restore 420
            {
                existing = _providers;
                added = existing.Concat(new[] { provider }).ToArray();
            }
        }

        /// <summary>
        /// Get the currently-active providers.
        /// </summary>
        /// <remarks>
        /// If the collection has been disposed, we'll leave the individual
        /// providers with the job of throwing <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public IEnumerable<ILoggerProvider> Providers => _providers;

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            foreach (var provider in _providers)
                provider.Dispose();
        }
    }
}