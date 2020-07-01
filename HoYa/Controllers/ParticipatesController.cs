using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System;
using System.Web;
using HoYa.Models;
using System.Data.Entity;

namespace HoYa.Controllers
{
    [Authorize]
    public class ParticipatesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetParticipate(Guid id)
        {
            Participate existedParticipate = await db.Participates.FindAsync(id);
            if (existedParticipate == null) return NotFound();
            return Ok(existedParticipate);
        }

        public async Task<IHttpActionResult> PutParticipate(Guid id, Participate participate)
        {
            Participate currentParticipate = await db.Participates.FindAsync(id);
            Activity currentActivity = await db.Activities.FindAsync(currentParticipate.OwnerId);
            Activity nextActivity = null;
            Step currentStep = await db.Steps.FindAsync(currentActivity.TargetId);
            Step nextStep = null;
            Process currentProcess = await db.Processes.FindAsync(currentActivity.OwnerId);

            //先確認這個參與者將用哪個RedirectId過關 
            Guid? redirectId = null;
            float redirectConditionCount = 0;
            float redirectConditionTrueRatio = 0;
            foreach (Redirect redirect in db.Redirects.Where(x => x.OwnerId == currentStep.Id).ToList().OrderBy(x => x.Target.Sort))
            {
                redirectConditionCount = 0;
                foreach (RedirectCondition redirectCondition in db.RedirectConditions.Where(x => x.OwnerId == redirect.Id).ToList().OrderBy(x => x.Sort))
                {
                    foreach (Judgment judgment in db.Judgments.Where(x => x.OwnerId == participate.Id).ToList())//取得所有判斷
                    {
                        if (judgment.ConditionId == redirectCondition.TargetId)
                        {
                            redirectConditionCount++;//條件符合即加一分
                        }
                    }
                    if (redirectConditionCount / db.RedirectConditions.Where(x => x.OwnerId == redirect.Id).Count() >= redirect.TrueRatio)//條件符合分數是否有達到原Step之Redirect通過比例
                    {
                        if (redirectConditionCount / db.RedirectConditions.Where(x => x.OwnerId == redirect.Id).Count() > redirectConditionTrueRatio)
                        {
                            redirectConditionTrueRatio = redirectConditionCount / db.RedirectConditions.Where(x => x.OwnerId == redirect.Id).Count();//有通過即設為最大通過比例 直到下一個更大通過比例的Redirect來取代
                            redirectId = redirect.Id;
                        }
                    }
                }
            }

            //取得Redirect後我們來設定下一關卡
            if (redirectId != null)
            {
                participate.RedirectId = redirectId;//設定通過的參與的通過原因
                participate.ArchivedDate = participate.EndDate;
                nextStep = db.Redirects.Find(redirectId).Target;//設定通過的參與的通過原因所指定的下個步驟

              //  int? ReEdit = currentActivity.ReEdit;//nextStep回關 ReEdit+1
              //  if (nextStep.Sort <= currentStep.Sort) ReEdit = ReEdit + 1;

                db.Entry(currentParticipate).CurrentValues.SetValues(participate);//送出此人的參與後
                await db.SaveChangesAsync();
                float? participateCount = 0;//總參與數量
                float? stepSubmitRatio = 0;//所有參與者通過比例
                //接下來要判斷此活動是否所有參與者都通過要進入下一節點 //先取得所有其他參與者且RedirectId等於目前的RedirectId
                foreach (Participate otherParticipate in db.Participates.Where(x => x.OwnerId == participate.OwnerId && participate.RedirectId == redirectId && x.ArchivedDate != null).ToList())
                {
                    participateCount++;
                    if (participateCount / db.Participates.Where(x => x.OwnerId == participate.OwnerId).Count() >= currentStep.SubmitRatio)
                    {
                        if (participateCount / db.Participates.Where(x => x.OwnerId == participate.OwnerId).Count() > stepSubmitRatio)
                        {
                            stepSubmitRatio = participateCount / db.Participates.Where(x => x.OwnerId == participate.OwnerId).Count();
                        }
                    }
                }

                if (stepSubmitRatio >= currentStep.SubmitRatio)//所有參與者通過比例 大於 活動設定比率 表示 全數通過 即送出
                {
                    nextActivity = db.Activities.FirstOrDefault(x =>
                    x.OwnerId == currentProcess.Id &&
                    x.TargetId == nextStep.Id
                    );


                    if (nextActivity == null)//原活動列表上沒有的新活動 加入新活動
                    {
                        nextActivity = new Activity
                        {
                            OwnerId = currentProcess.Id,
                            TargetId = nextStep.Id,
                          //  ReEdit = ReEdit,//給 General找版本用
                            PreviousId = currentActivity.Id
                        };
                        db.Activities.Add(nextActivity);
                        //   await db.SaveChangesAsync();
                        db.SaveChanges();
                    }






                    //更新目前活動
                    Activity existedCurrentActivity = await db.Activities.FindAsync(currentActivity.Id);
                    if (currentActivity.StartDate == null) currentActivity.StartDate = participate.ArchivedDate;
                    currentActivity.EndDate = participate.ArchivedDate;
                    currentActivity.ArchivedDate = currentActivity.EndDate;
                    currentActivity.NextId = nextActivity.Id;
                    currentActivity.ArchivedParticipateId = participate.Id;//因此 參與 而 結束了 此活動
                    db.Entry(existedCurrentActivity).CurrentValues.SetValues(currentActivity);
                    await db.SaveChangesAsync();

                    //更新目前流程
                    Process existedProcess = await db.Processes.FindAsync(currentProcess.Id);
                   // currentProcess.ReEdit = ReEdit;
                    currentProcess.PreviousId = existedProcess.CurrentId;
                    currentProcess.CurrentId = nextActivity.Id;
                    db.Entry(existedProcess).CurrentValues.SetValues(currentProcess);
                    await db.SaveChangesAsync();

                    //更新下個活動
                    Activity existedNextActivity = await db.Activities.FindAsync(nextActivity.Id);
                    nextActivity.StartDate = currentActivity.EndDate;
                   // nextActivity.ReEdit = ReEdit;
                    nextActivity.PreviousId = currentActivity.Id;
                    db.Entry(existedNextActivity).CurrentValues.SetValues(nextActivity);

                    //增加下個活動的參與
                    //先判斷nextStep是任務還是指派
                    if (nextStep.Type.Value == "指派")
                    {
                        //群組指派
                        foreach (StepGroup stepGroup in db.StepGroups.Where(x => x.OwnerId == nextStep.Id).ToList().OrderBy(x => x.Sort))
                        {

                            foreach (Relationship inventoryGroup in db.Relationships.Where(x => x.TargetId == stepGroup.TargetId).ToList().OrderBy(x => db.InventoryAttributes.FirstOrDefault(y => y.OwnerId == x.Id && y.Target.Value == "Sort").Value))
                            {
                                Participate newParticipate = new Participate
                                {
                                    OwnerId = nextActivity.Id,
                                    ParticipantId = inventoryGroup.OwnerId
                                };
                                db.Participates.Add(newParticipate);
                            }
                        }

                        //關卡關係人指派
                        foreach (StepRelationship stepRelationship in db.StepRelationships.Where(x => x.OwnerId == nextStep.Id).ToList().OrderBy(x => x.Sort))
                        {
                            Inventory participant = null;
                            Activity refActivity = db.Activities.FirstOrDefault(x =>
                            x.OwnerId == currentActivity.OwnerId &&
                            x.TargetId == stepRelationship.OfId
                            );
                            if (refActivity != null)
                            {

                                Inventory relationshipOwner = refActivity.ArchivedParticipate.Participant;
                                Relationship refRelationship = db.Relationships.FirstOrDefault(x =>
                                                            x.OwnerId == relationshipOwner.Id && //誰的 ex:李芳賢的
                                                            x.TypeId == stepRelationship.TargetId //誰 ex:代理人
                                                        );
                                if (refRelationship != null)
                                {
                                    participant = refRelationship.Target;
                                }
                                else{
                                    participant= refActivity.ArchivedParticipate.Participant;
                                }
                            }



                            //給新活動 綁定 新參與 和 參與人
                            Participate newParticipate = new Participate
                            {
                                OwnerId = nextActivity.Id,
                                ParticipantId = participant.Id
                            };
                            db.Participates.Add(newParticipate);

                        }
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        //使用者在前端接任務時才寫入newParticipate
                    }
                }
            }
            await db.SaveChangesAsync();
            await db.Entry(currentParticipate).GetDatabaseValuesAsync();
            return Ok(currentParticipate);
        }

        public async Task<IHttpActionResult> GetParticipates(
            string anyLike = "",
            bool? archivedDateIsNull = false,
            Guid? ownerId = null,
            Guid? participantId = null,
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
            )
        {
            IQueryable<Participate> participates = db.Participates.Where(x =>
             (x.OwnerId == ownerId || ownerId == null) &&
             ((x.ArchivedDate == null) == archivedDateIsNull) &&
             (x.ParticipantId == participantId || participantId == null)
            );
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Participate>
                    {
                        PaginatorLength = participates.Count(),
                        Data = participates.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Participate>()
                    });
                    else return Ok(new Query<Participate>
                    {
                        PaginatorLength = participates.Count(),
                        Data = participates.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Participate>()
                    });
            }
        }

        public async Task<IHttpActionResult> PostParticipate(Participate participate)
        {
            db.Participates.Add(participate);
            await db.SaveChangesAsync();
            await db.Entry(participate).GetDatabaseValuesAsync();
            participate.Owner = await db.Activities.FindAsync(participate.OwnerId);
            return Ok(participate);
        }

        public async Task<IHttpActionResult> DeleteParticipate(Guid id)
        {
            Participate existedParticipate = await db.Participates.FindAsync(id);
            if (existedParticipate == null) return NotFound();
            db.Participates.Remove(existedParticipate);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}