using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Searchify.Services.InvertedIndex;

namespace Searchify.DynamoDb.Models
{
    public class InvertedIndexModel
    {
        public static async Task<uint> GetLastId()
        {
            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
                ["Term"] = new AttributeValue { S = "last.id" }
            };

            var request = new GetItemRequest { TableName = "inverted_index", Key = key };

            GetItemResponse response;
            response = await DbClient.Client.GetItemAsync(request);
            Dictionary<string, AttributeValue> item = response.Item;
            try
            {
                return UInt32.Parse(item["id"].N);

            }
            catch (KeyNotFoundException e)
            {
                return 0;
            }
        }

        public static async Task<PutItemResponse> SetLastId(uint lastId)
        {
            Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>
            {
                ["Term"] = new AttributeValue { S = "last.id" },
                ["id"] = new AttributeValue { N = lastId.ToString() }
            };

            var request = new PutItemRequest
            {
                TableName = "inverted_index",
                Item = attributes
            };

            return await DbClient.Client.PutItemAsync(request);
        }

        public static bool CheckTermIndexed(string term)
        {
            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
                ["Term"] = new AttributeValue { S = term }
            };

            var request = new GetItemRequest { TableName = "inverted_index", Key = key };

            try
            {
                var response =  DbClient.Client.GetItemAsync(request).GetAwaiter().GetResult().Item;
                Dictionary<string, AttributeValue> item = response;
                return item["TermList"].L != null;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public static async Task<HttpStatusCode> AppendIndexTerm(string term, IndexTerm indexTerm)
        {
            List<AttributeValue> positionsAttrs = new List<AttributeValue>();

            foreach (var indexTermPosition in indexTerm.Positions)
            {
                positionsAttrs.Add(new AttributeValue { N = indexTermPosition.ToString() });
            }

            if (!CheckTermIndexed(term))
            {
                Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>
                {
                    ["Term"] = new AttributeValue { S = term },
                    ["TermList"] = new AttributeValue
                    {
                        L = new List<AttributeValue>
                        {
                            new AttributeValue // indexTerm
                            {
                                M = new Dictionary<string, AttributeValue>
                                {
                                    ["fileDelta"] = new AttributeValue{ N = indexTerm.FileDelta.ToString() },
                                    ["positions"] = new AttributeValue{ L = positionsAttrs }
                                }
                            }
                        }
                    }
                };

                PutItemRequest request = new PutItemRequest
                {
                    TableName = "inverted_index",
                    Item = attributes
                };

                var response = await DbClient.Client.PutItemAsync(request);

                return response.HttpStatusCode;
            }
            else
            {
                Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
                {
                    ["Term"] = new AttributeValue { S = term }
                };

                Dictionary<string, AttributeValueUpdate> updates = new Dictionary<string, AttributeValueUpdate>
                {
                    ["TermList"] = new AttributeValueUpdate
                    {
                        Action = AttributeAction.ADD,
                        Value = new AttributeValue
                        {
                            L = new List<AttributeValue>
                            {
                                new AttributeValue
                                {
                                    M = new Dictionary<string, AttributeValue>
                                    {
                                        ["fileDelta"] = new AttributeValue{ N = indexTerm.FileDelta.ToString() },
                                        ["positions"] = new AttributeValue{ L = positionsAttrs }
                                    }
                                }
                            }
                        }
                    }
                };

                UpdateItemRequest request = new UpdateItemRequest
                {
                    TableName = "inverted_index",
                    Key = key,
                    AttributeUpdates = updates
                };

                var response = await DbClient.Client.UpdateItemAsync(request);

                return response.HttpStatusCode;
            }
        }

        public static async Task<List<IndexTerm>> GetIndexTermList(string term)
        {
            List<IndexTerm> indexTerms = new List<IndexTerm>();

            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
                ["Term"] = new AttributeValue { S = term }
            };

            var request = new GetItemRequest { TableName = "inverted_index", Key = key };

            try
            {
                var response = await DbClient.Client.GetItemAsync(request);
                Dictionary<string, AttributeValue> item = response.Item;
                foreach (var attributeValue in item["TermList"].L)
                {
                    Dictionary<string, AttributeValue> indexTermMap = attributeValue.M;
                    uint fileDelta = uint.Parse(indexTermMap["fileDelta"].N);
                    List<uint> positions = new List<uint>();
                    foreach (var value in indexTermMap["positions"].L)
                    {
                        positions.Add(uint.Parse(value.N));
                    }
                    IndexTerm indexTerm = new IndexTerm(fileDelta);
                    indexTerm.AddPositions(positions.ToArray());
                    indexTerms.Add(indexTerm);
                }
            }
            catch (KeyNotFoundException)
            {
            }
            return indexTerms;
        }
    }
}