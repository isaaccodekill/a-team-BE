using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Searchify.Domain.Utils;


namespace SearchTest
{
    public class UtilsTest
    {
        [Fact]
        public void TestTokenizer_Util()
        {
            // arrange 
            var text = "Ablity to fight";
            var tokens = Tokenizer.Tokenize(text);
            Assert.Equal(2, tokens.Length);
            Assert.Equal("abliti", tokens[0].ToLower());

        }

        [Fact]
        public void Test_Stopword_util()
        {
            // arrange 
            var text = "There are a lot of stopwords to be removed";
            var tokens = Stopwords.Clean(text);
            Assert.Equal(4, tokens.Count);
            Assert.Equal("there", tokens[0].ToLower());

        }

        [Fact]
        public void Test_Textcleaner_util()
        {
            // arrange 
            var text = "Clean    My text!!!!!!!!!!!!!!!";
            var cleanedText = Utils.CleanText(text);
            Assert.Equal("clean my text", cleanedText);

        }
    }
}
