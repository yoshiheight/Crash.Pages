using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Crash.Core.IO
{
    /// <summary>
    /// YAMLシリアライザ
    /// </summary>
    public static class YamlConvert
    {
        /// <summary>
        /// 文書内の先頭のYAMLをデシリアライズする
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static TResult DeserializeFirst<TResult>(string text)
        {
            using (var reader = new StringReader(text))
            {
                return DeserializeFirst<TResult>(reader);
            }
        }

        /// <summary>
        /// 文書内の先頭のYAMLをデシリアライズする
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static TResult DeserializeFirst<TResult>(TextReader reader)
        {
            var parser = new Parser(reader);
            parser.Expect<StreamStart>();

            var deserializer = new DeserializerBuilder().Build();
            if (parser.Accept<DocumentStart>())
            {
                return deserializer.Deserialize<TResult>(parser);
            }

            throw new ArgumentException();
        }
    }
}
