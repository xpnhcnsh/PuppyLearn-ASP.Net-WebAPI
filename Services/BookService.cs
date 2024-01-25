using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PuppyLearn.Models;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;
using System.Net;

namespace PuppyLearn.Services
{
    public class BookService : IBookService
    {
        private readonly PuppyLearnContext _context;

        public BookService(PuppyLearnContext context)
        {
            _context = context;
        }

        public async Task<ReturnValue> AddbyFolderUrl(string url)
        {

            if (Directory.Exists(url))
            {
                var filesFullPath = Directory.GetFiles(url,"*.json").ToList();
                foreach(var fileFullPath in filesFullPath)
                {
                    using StreamReader sr = new StreamReader(fileFullPath);
                    var jsonBook = await sr.ReadToEndAsync();
                    dynamic jsonBookObj = JsonConvert.DeserializeObject(jsonBook);
                    var wordsCount = jsonBookObj.Count;

                    #region get data for table Books_en and insert.
                    var bookName = (string)jsonBookObj.First.bookId.Value;
                    if (_context.BooksEns.Select(a=>a.BookName).ToList().Contains(bookName))
                    {
                        continue;
                    }
                    await Console.Out.WriteLineAsync(bookName);
                    var book = new BooksEn
                    {
                        Id = Guid.NewGuid(),
                        BookName = bookName,
                        WordsCount = wordsCount
                    };
                    await _context.BooksEns.AddAsync(book);
                    #endregion

                    #region insert into all other tables for each word interating
                    foreach (var wordObj in jsonBookObj)
                    {
                        await Console.Out.WriteLineAsync((string)wordObj.headWord.Value);
                        string ukphone = (wordObj.content.word.content["ukphone"] != null) ? (string)wordObj.content.word.content.ukphone.Value : "";
                        string usphone = (wordObj.content.word.content["usphone"] != null) ? (string)wordObj.content.word.content.usphone.Value : "";
                        string ukspeech = (wordObj.content.word.content["ukspeech"] != null) ? (string)wordObj.content.word.content.ukspeech.Value : "";
                        string usspeech = (wordObj.content.word.content["usspeech"] != null) ? (string)wordObj.content.word.content.usspeech.Value : "";
                        var wordsEntry = new Word
                        {
                            Id = Guid.NewGuid(),
                            WordName = (string)wordObj.headWord.Value,
                            BookId = book.Id,
                            Ukphone = ukphone,
                            Usphone = usphone,
                            Ukspeech = ukspeech,
                            Usspeech = usspeech,
                        };

                        #region table sentences
                        if (wordObj.content.word.content["sentence"] != null)
                        {
                            var setences = wordObj.content.word.content.sentence.sentences;
                            foreach (var sentence in setences)
                            {
                                string sentenceCn = (sentence["sCn"] != null) ? (string)sentence.sCn.Value : "";
                                string sentenceEn = (sentence["sContent"] != null) ? (string)sentence.sContent.Value : "";

                                var sentencesEntry = new Sentence
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordsEntry.Id,
                                    BookId = book.Id,
                                    SentenceCn = sentenceCn,
                                    SentenceEn = sentenceEn,
                                };
                                await _context.Sentences.AddAsync(sentencesEntry);
                            }
                        }

                        #endregion

                        #region table Synonymous
                        if (wordObj.content.word.content["syno"] != null)
                        {
                            var synos = wordObj.content.word.content.syno.synos;
                            foreach (var syno in synos)
                            {
                                var hwds = syno.hwds;
                                foreach (var hwd in hwds)
                                {
                                    string pos = (syno["pos"] != null) ? (string)syno.pos.Value : "";
                                    string transCn = (syno["tran"] != null) ? (string)syno.tran.Value : "";
                                    string synoEn = (hwd["w"] != null) ? (string)hwd.w.Value : "";

                                    var synosEntry = new Synonymou
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordsEntry.Id,
                                        BookId = book.Id,
                                        Pos = pos,
                                        TransCn = transCn,
                                        SynoEn = synoEn
                                    };
                                    await _context.Synonymous.AddAsync(synosEntry);
                                }
                            }
                        }
                        #endregion

                        #region table Phrases
                        if (wordObj.content.word.content["phrase"] != null)
                        {
                            var phrases = wordObj.content.word.content.phrase.phrases;
                            foreach (var phrase in phrases)
                            {
                                string phraseEn = (phrase["pContent"] != null) ? (string)phrase.pContent.Value : "";
                                string phraseCn = (phrase["pCn"] != null) ? (string)phrase.pCn.Value : "";
                                var phrasesEntry = new Phrase
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordsEntry.Id,
                                    BookId = book.Id,
                                    PhraseEn = phraseEn,
                                    PhraseCn = phraseCn
                                };
                                await _context.Phrases.AddAsync(phrasesEntry);
                            }
                        }


                        #endregion

                        #region table Cognate
                        if (wordObj.content.word.content["relWord"] != null)
                        {
                            var cognates = wordObj.content.word.content.relWord.rels;
                            foreach (var cognate in cognates)
                            {
                                var words = cognate.words;
                                foreach (var word in words)
                                {
                                    string cognateCn = (word["tran"] != null) ? (string)word.tran.Value : "";
                                    string cognateEn = (word["hwd"] != null) ? (string)word.hwd.Value : "";
                                    string pos = (cognate["pos"] != null) ? (string)cognate.pos.Value : "";

                                    var cognatesEntry = new Cognate
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordsEntry.Id,
                                        BookId = book.Id,
                                        CognateCn = cognateCn,
                                        CognateEn = cognateEn,
                                        Pos = pos
                                    };
                                    await _context.Cognates.AddAsync(cognatesEntry);
                                }
                            }
                        }

                        #endregion

                        #region table Trans
                        if (wordObj.content.word.content["trans"] != null)
                        {
                            var trans = wordObj.content.word.content.trans;
                            foreach (var tran in trans)
                            {
                                string pos;
                                string transCn;
                                string transEn;

                                pos = (tran["pos"] != null) ? (string)tran.pos.Value : "";
                                transCn = (tran["tranCn"] != null) ? (string)tran.tranCn.Value : "";
                                transEn = (tran["tranOther"] != null) ? (string)tran.tranCn.Value : "";

                                var transEntry = new Tran
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordsEntry.Id,
                                    BookId = book.Id,
                                    TransCn = transCn,
                                    TransEn = transEn,
                                    Pos = pos,
                                };
                                await _context.Trans.AddAsync(transEntry);
                            }
                        }

                        #endregion

                        #region table SingleChoiceQuestions
                        if (wordObj.content.word.content["exam"] != null)
                        {
                            var questions = wordObj.content.word.content.exam;
                            foreach (var q in questions)
                            {
                                string questionEn = (q["question"] != null) ? (string)q.question.Value : "";
                                string ansExplainCn = (q["answer"]["explain"] != null) ? (string)q.answer.explain.Value : "";
                                int answerIndex = (q["answer"]["rightIndex"] != null) ? (int)q.answer.rightIndex.Value : -1;
                                string choice1 = (q["choices"][0] != null) ? (string)q.choices[0].choice.Value : "";
                                string choice2 = (q["choices"][1] != null) ? (string)q.choices[1].choice.Value : "";
                                string choice3 = (q["choices"][2] != null) ? (string)q.choices[2].choice.Value : "";
                                string choice4 = (q["choices"][3] != null) ? (string)q.choices[3].choice.Value : "";

                                var singleChoiceQuestionsEntry = new SingleChoiceQuestion
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordsCount.Id,
                                    BookId = book.Id,
                                    QuestionEn = questionEn,
                                    AnsExplainCn = ansExplainCn,
                                    AnswerIndex = answerIndex,
                                    Choice1 = choice1,
                                    Choice2 = choice2,
                                    Choice3 = choice3,
                                    Choice4 = choice4,
                                };
                                await _context.SingleChoiceQuestions.AddAsync(singleChoiceQuestionsEntry);
                            }
                        }
                        #endregion
                        await _context.Words.AddAsync(wordsEntry);
                        }
                        #endregion


                        //foreach (var jsonBoookObj in jsonBookObjs)
                        //{
                        //    var word = new
                        //    {
                        //        bookName = jsonBoookObj.bookId.split("_")[0],

                        //    };
                        //} 
                        await _context.SaveChangesAsync();  
                }
                //var res = JsonConvert.SerializeObject(booksEntoUpdate);
                return new ReturnValue
                {
                    Value = null,
                    HttpCode = 200,
                    Msg = "找到路径，返回文件名列表"
                };

            }
            else
            {
                return new ReturnValue
                {
                    Value = null,
                    HttpCode = 404,
                    Msg = "文件夹不存在"
                };
            }
        }
    }
}
