using GmailServer.EmailChecks;
using GmailServer.Enums;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailServer.TaskPools
{
    public class TaskPool
    {
        public int MaxThread { get; set; }

        private List<EmailCheck> EmailCheck { get; set; }
        private List<EmailResult> Result { get; set; }
        private RestClient client;

        public TaskPool()
        {
            EmailCheck = new List<EmailCheck>();
            Result = new List<EmailResult>();
            client = new RestClient("https://peoplestack-pa.clients6.google.com/");
            client.CookieContainer.Add(new System.Net.Cookie("__Secure-3PSID", "Kghzx3B9eKvHsa4AZG8qvbOCvm-pA5OCMEbZLZ4gfIgOCIilF_4-ttx-oIF-OtpDOcSAmw.", "/", ".google.com"));
            client.CookieContainer.Add(new System.Net.Cookie("__Secure-3PAPISID", "tcdKkjomeIutgUNC/AJL0iS6LL4lPvHtOU", "/", ".google.com"));
            MaxThread = 100;
            ServicePointManager.DefaultConnectionLimit = 1000000;
        }

        public void Start()
        {
            new Task(() => StartThread()).Start();
        }

        public void EnqueueEmail(string email, long ID)
        {
            lock (EmailCheck)
            {
                EmailCheck.Add(new EmailCheck()
                {
                    Id = ID,
                    Email = email
                });
            }
        }


        public void EnqueueEmail(EmailCheck email)
        {
            lock (EmailCheck)
            {
                EmailCheck.Add(email);
            }
        }
        public void EnqueueEmails(List<EmailCheck> emails)
        {
            lock (EmailCheck)
            {
                EmailCheck.AddRange(emails);
            }

        }

        public List<EmailCheck> DequeueEmails()
        {
            lock (EmailCheck)
            {
                if (EmailCheck.Count == 0) return new List<EmailCheck>();
                var _ret = EmailCheck.ToList();
                EmailCheck.Clear();
                return _ret;
            }
        }
        public List<EmailResult> GetResult()
        {
            lock (Result)
            {
                return Result;
            }
        }

        public List<EmailResult> GetResultAndClear()
        {
            lock (Result)
            {
                var ret = Result.ToList();
                Result.Clear();
                return ret;
            }
        }

        private EmailResult CheckGmail(string mail, long index = 0)
        {
            var request = new RestRequest("$rpc/peoplestack.PeopleStackAutocompleteService/Lookup", Method.Post);
            request.Timeout = 20000; // 20s
            request.AddHeader("content-type", "application/json+protobuf");
            request.AddHeader("authorization", "SAPISIDHASH 1655096897_451e0dc938a3af644981cc70a429315a18219b1d");
            request.AddHeader("origin", "https://drive.google.com");
            request.AddHeader("x-goog-api-key", "AIzaSyC4JjdyoZPBZbhiXypJRsdhGicms9lgzoA");
            request.AddBody($"[58,[1],[[\"{mail}\"]]]");
        retry:
            //var attempts = 0;
            //while (attempts < 3)
            //{
            //    attempts++;
            var response = client.Execute(request);
            if (response.Content != null)
            {
                if (response.Content.Contains("googleusercontent"))
                {
                    return new EmailResult()
                    {
                        Email = mail,
                        Id = index,
                        Status = Status.Good

                    };
                }

                if (response.Content.ToLower().Contains(mail.ToLower()))
                {
                    return new EmailResult()
                    {
                        Email = mail,
                        Id = index,
                        Status = Status.Verify
                    };
                }
            }

            if (response.StatusCode == HttpStatusCode.RequestTimeout)
            {
                return new EmailResult()
                {
                    Email = mail,
                    Id = index,
                    Status = Status.Unknown
                };
            }
            //    Thread.Sleep(500);
            //}

            //return new EmailResult()
            //{
            //    Email = mail,
            //    Id = index,
            //    Status = Status.Unknown
            //};
            Thread.Sleep(500);
            goto retry;

        }

        public void StartCheckWithEmailChecks(List<EmailCheck> emailChecks)
        {
            var checkMailTasks = new List<Task>();

            for (int i = 0; i < emailChecks.Count; i++)
            {
                var email = emailChecks[i];
                var task = new Task(() =>
                {
                    var _ret = CheckGmail(email.Email, email.Id);
                    lock (Result)
                    {
                        Result.Add(_ret);
                    }

                });
                checkMailTasks.Add(task);
                task.Start();
                if (checkMailTasks.Count >= MaxThread)
                {
                    Task.WaitAny(checkMailTasks.ToArray());
                    checkMailTasks.Where(x => x.IsCompleted).ToList().ForEach(y =>
                    {
                        checkMailTasks.Remove(y);
                        y.Dispose();
                    });
                }
            }
        }

        public void StartThread()
        {
            var CheckGmailsTasks = new List<Task>();
            var EmailsToCheck = new List<EmailCheck>();
            while (true)
            {
                EmailsToCheck.AddRange(DequeueEmails());
                if (EmailsToCheck.Count > 0)
                {
                    for (int i = 0; i < EmailsToCheck.Count; i++)
                    {
                        var email = EmailsToCheck[i];
                        var task = new Task(() =>
                        {
                            var _ret = CheckGmail(email.Email, email.Id);
                            lock (Result)
                            {
                                Result.Add(_ret);
                            }

                        });
                        CheckGmailsTasks.Add(task);
                        task.Start();
                        if (CheckGmailsTasks.Count >= MaxThread)
                        {
                            Task.WaitAny(CheckGmailsTasks.ToArray());
                            CheckGmailsTasks.Where(x => x.IsCompleted).ToList().ForEach(y =>
                            {
                                CheckGmailsTasks.Remove(y);
                                y.Dispose();
                            });
                        }


                    }
                    EmailsToCheck.Clear();
                }
                else
                {
                    if (CheckGmailsTasks.Count > 0)
                    {
                        Task.WaitAny(CheckGmailsTasks.ToArray());
                        CheckGmailsTasks.Where(x => x.IsCompleted).ToList().ForEach(y =>
                        {
                            CheckGmailsTasks.Remove(y);
                            y.Dispose();
                        });
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
