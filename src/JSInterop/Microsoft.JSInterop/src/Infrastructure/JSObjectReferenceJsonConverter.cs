// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop.Implementation;

namespace Microsoft.JSInterop.Infrastructure
{
    internal sealed class JSObjectReferenceJsonConverter : JsonConverter<IJSObjectReference>
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert == typeof(IJSObjectReference) || typeToConvert == typeof(JSObjectReference);

        public override IJSObjectReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsRuntime = JSRuntimeProvider.GetJSRuntime(options);

            var id = JSObjectReferenceJsonWorker.ReadJSObjectReferenceIdentifier(ref reader);
            return new JSObjectReference(jsRuntime, id);
        }

        public override void Write(Utf8JsonWriter writer, IJSObjectReference value, JsonSerializerOptions options)
        {
            JSObjectReferenceJsonWorker.WriteJSObjectReference(writer, (JSObjectReference)value);
        }
    }
}
