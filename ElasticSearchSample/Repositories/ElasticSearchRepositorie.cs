using ElasticSearchSample.Models;
using Microsoft.Extensions.Configuration;
using Nest;
using System;

namespace ElasticSearchSample.Repositories
{
    public class ElasticSearchRepositorie
    {
        public static Uri node;
        public static ConnectionSettings settings;
        public static ElasticClient client;
        private readonly IConfiguration _config;

        public ElasticSearchRepositorie(IConfiguration config)
        {
            _config = config;

            node = new Uri(_config["ElasticSearchUri"]);
            settings = new ConnectionSettings(node);
            settings.DefaultIndex("contentidx");
            client = new ElasticClient(settings);

            var indexSettings = new IndexSettings
            {
                NumberOfReplicas = 1,
                NumberOfShards = 1
            };

            if (!client.IndexExists("contentidx").Exists)
            {
                var createIndexResponse = client.CreateIndex("contentidx", c => c
                    .Mappings(ms => ms
                        .Map<Content>(m => m
                            .AutoMap(typeof(Content))
                        )
                    )
                );
            }
        }

        public ISearchResponse<Content> GetByTerm(string term)
        {
            var result = client.Search<Content>(s =>
                   s
                   .From(0)
                   .Size(10000)
                   .Type("content")
                   .Query(q => q.Match(mq => mq.Field(f => f.ContentText).Query(term))));

            return result;
        }

        public ISearchResponse<Content> GetById(int id)
        {
            var result = client.Search<Content>(s =>
                s.From(0).Size(10000).Type("content").Query(q => q.Term(t => t.ContentId, id)));

            return result;
        }

        public void Insert(Content content)
        {
            content.PostDate = DateTime.Now;
            client.Index(content, i => i.Index("contentidx"));
        }

        //public void DeleteIndex()
        //{
        //    client.DeleteIndex("contentidx");
        //}
    }
}
