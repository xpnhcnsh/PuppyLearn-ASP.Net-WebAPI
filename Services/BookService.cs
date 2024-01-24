using Microsoft.AspNetCore.Authorization.Infrastructure;
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
                    if (fileFullPath.Contains("BEC")) 
                    {
                        using StreamReader sr = new StreamReader(fileFullPath);
                        var jsonBook = await sr.ReadToEndAsync();
                        dynamic jsonBookObj = JsonConvert.DeserializeObject(jsonBook);
                        var wordsCount = jsonBookObj.Count;
                        var sentencesEntrys = new List<Sentence>();
                        var synosEntrys = new List<Synonymou>();
                        var phrasesEntrys = new List<Phrase>();
                        var cognatesEntrys = new List<Cognate>();
                        var transEntrys = new List<Tran>();
                        var singleChoiceQuestionsEntrys = new List<SingleChoiceQuestion>();

                        #region get data for table Books_en and insert.
                        var bookName = (string)jsonBookObj.First.bookId.Value;
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
                            var wordsEntry = new Word
                            {
                                Id = Guid.NewGuid(),
                                WordName = (string)wordObj.headWord.Value,
                                BookId = book.Id,
                                Ukphone = (string)wordObj.content.word.content.ukphone.Value,
                                Usphone = (string)wordObj.content.word.content.usphone.Value,
                                Ukspeech = (string)wordObj.content.word.content.ukspeech.Value,
                                Usspeech = (string)wordObj.content.word.content.usspeech.Value,
                            };

                            #region table sentences
                            if (wordObj.content.word.content["sentence"] != null) 
                            {
                                var setences = wordObj.content.word.content.sentence.sentences;
                                foreach (var sentence in setences)
                                {
                                    var sentencesEntry = new Sentence
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordsEntry.Id,
                                        BookId = book.Id,
                                        SentenceCn = (string)sentence.sCn.Value,
                                        SentenceEn = (string)sentence.sContent.Value,
                                    };
                                    sentencesEntrys.Add(sentencesEntry);
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
                                        var synosEntry = new Synonymou
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = wordsEntry.Id,
                                            BookId = book.Id,
                                            Pos = (string)syno.pos.Value,
                                            TransCn = (string)syno.tran.Value,
                                            SynoEn = (string)hwd.w.Value
                                        };
                                        synosEntrys.Add(synosEntry);
                                        await _context.Synonymous.AddAsync(synosEntry);
                                    }
                                }
                            }
                            #endregion

                            #region table Phrases
                            if (wordObj.content.word.content["phrase"]!=null)
                            {
                                var phrases = wordObj.content.word.content.phrase.phrases;
                                foreach (var phrase in phrases)
                                {
                                    var phrasesEntry = new Phrase
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordsEntry.Id,
                                        BookId = book.Id,
                                        PhraseEn = (string)phrase.pContent.Value,
                                        PhraseCn = (string)phrase.pCn.Value
                                    };
                                    phrasesEntrys.Add(phrasesEntry);
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
                                        var cognatesEntry = new Cognate
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = wordsEntry.Id,
                                            BookId = book.Id,
                                            CognateCn = (string)word.tran.Value,
                                            CognateEn = (string)word.hwd.Value,
                                            Pos = (string)cognate.pos.Value
                                        };
                                        cognatesEntrys.Add(cognatesEntry);
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
                                    if (tran["pos"]!=null)
                                    {
                                        var transEntry = new Tran
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = wordsEntry.Id,
                                            BookId = book.Id,
                                            TransCn = (string)tran.tranCn.Value,
                                            TransEn = (string)tran.tranOther.Value,
                                            Pos = (string)tran.pos.Value,
                                        };
                                        transEntrys.Add(transEntry);
                                        await _context.Trans.AddAsync(transEntry);
                                    }
                                    else
                                    {
                                        var transEntry = new Tran
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = wordsEntry.Id,
                                            BookId = book.Id,
                                            TransCn = (string)tran.tranCn.Value,
                                            TransEn = (string)tran.tranOther.Value,
                                        };
                                        transEntrys.Add(transEntry);
                                        await _context.Trans.AddAsync(transEntry);
                                    } 
                                }
                            }

                            #endregion

                            #region table SingleChoiceQuestions
                            if (wordObj.content.word.content["exam"] != null) 
                            {
                                var questions = wordObj.content.word.content.exam;
                                foreach (var q in questions)
                                {
                                    var singleChoiceQuestionsEntry = new SingleChoiceQuestion
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordsCount.Id,
                                        BookId = book.Id,
                                        QuestionEn = (string)q.question.Value,
                                        AnsExplainCn = (string)q.answer.explain.Value,
                                        AnswerIndex = (int)q.answer.rightIndex.Value,
                                        Choice1 = (string)q.choices[0].choice.Value,
                                        Choice2 = (string)q.choices[1].choice.Value,
                                        Choice3 = (string)q.choices[2].choice.Value,
                                        Choice4 = (string)q.choices[3].choice.Value,
                                    };
                                    singleChoiceQuestionsEntrys.Add(singleChoiceQuestionsEntry);
                                    await _context.SingleChoiceQuestions.AddAsync(singleChoiceQuestionsEntry);
                                }
                            }
                            #endregion

                            #region now fulfill all FKs for table Words

                            sentencesEntrys?.ForEach(sentencesEntry =>
                            {
                                synosEntrys?.ForEach(synosEntry =>
                                {
                                    phrasesEntrys?.ForEach(phrasesEntry => 
                                    {
                                        cognatesEntrys?.ForEach(cognatesEntry => 
                                        {
                                            transEntrys?.ForEach(transEntry => 
                                            {
                                            singleChoiceQuestionsEntrys?.ForEach(singleChoiceQuestionsEntry=>
                                            {
                                                wordsEntry.SentenceId = sentencesEntry.Id;
                                                wordsEntry.SynoId = synosEntry.Id;
                                                wordsEntry.PhraseId = phrasesEntry.Id;
                                                wordsEntry.CognateId = cognatesEntry.Id;
                                                wordsEntry.TransId = transEntry.Id;
                                                wordsEntry.SingleChoiceQuestionId = singleChoiceQuestionsEntry.Id;
                                            });
                                            });
                                        });
                                    });
                                });
                            });
                            await _context.Words.AddAsync(wordsEntry);
                            #endregion
                        }
                        await _context.SaveChangesAsync();

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
