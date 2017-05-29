using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.DTO;

namespace ElectoralCalculator.Models.Database
{
    public class DatabaseManager : IDatabaseManager
    {
        private static DatabaseManager _instance;
        public static DatabaseManager Instance =>
            _instance ?? (_instance = new DatabaseManager());

        public async Task<int?> GetCurrentTimestamp()
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    var dateTime = await context.Database.SqlQuery<DateTime>("SELECT GETDATE();").FirstOrDefaultAsync();
                    if (dateTime == null)
                        return null;
                    return (int?)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreatePersonAsync(Person person)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    context.Persons.Add(person);
                    context.Entry(person).State = System.Data.Entity.EntityState.Added;

                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Person GetPerson(string pesel)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                    return context.Persons.SingleOrDefault(p => p.Pesel == pesel);
            }
            catch
            {
                return null;
            }
        }

        public Candidate GetCandidate(string name)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                    return context.Candidates.First(p => p.Name.ToLower() == name.ToLower());
            }
            catch
            {
                return null;
            }
        }

        public Candidate GetCandidate(int id)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                    return context.Candidates.First(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }


        public async Task<bool> IncrementVotesNumAsync(bool isVoteGood)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    var vote = context.Votes.Single();

                    if (isVoteGood)
                        vote.Valid_Votes++;
                    else
                        vote.Bad_Votes++;

                    context.Entry(vote).State = System.Data.Entity.EntityState.Modified;

                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IncrementPersonWithLawTriedToVote()
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    var vote = context.Votes.Single();
                    vote.WithoutLaw_TriesNum++;

                    context.Entry(vote).State = System.Data.Entity.EntityState.Modified;

                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<StatsNonDb> GetStatsAsync()
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    var stats = new StatsNonDb();
                    var parties = context.Parties;

                    await Task.Run(() =>
                    {
                        parties.ToList().ForEach(p =>
                        {
                            stats.Parties.Add(new PartyStatsNonDb()
                            {
                                Id = p.Id,
                                Name = p.Name,
                                VotesNum = p.Candidates.SelectMany(d => d.Persons).Count()
                            });

                            stats.Candidates.AddRange(p.Candidates.Select(d =>
                                new CandidateStatsNonDb()
                                {
                                    PartyId = d.PartyId,
                                    Name = d.Name,
                                    VotesNum = d.Persons.Count
                                }));
                        });

                        var votes = context.Votes.First();
                        stats.UnvalidVotesNun = votes.Bad_Votes;
                        stats.ValidVotesNum = votes.Valid_Votes;
                        stats.WithoutLawTries = votes.WithoutLaw_TriesNum;

                    });

                    return stats;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateSessionAsync(string pesel, string key, int timestamp)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    var service = context.LoginServices.FirstOrDefault(p => p.pesel == pesel);

                    if (service == null)
                    {
                        var loginService = new LoginService()
                        {
                            guid = key,
                            timestamp = timestamp,
                            pesel = pesel
                        };
                        context.LoginServices.Add(loginService);
                        context.Entry(loginService).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        service.timestamp = timestamp;
                        service.guid = key;
                        context.Entry(service).State = System.Data.Entity.EntityState.Modified;
                    }

                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<LoginService> GetSession(string pesel)
        {
            try
            {
                using (var context = new ElectoralDatabase())
                {
                    var service = await Task.Run(() => context.LoginServices.FirstOrDefault(p => p.pesel == pesel));
                    return service;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
