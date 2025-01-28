using AutoMapper;
using Data;
using Dto;
using Dto.ModelAi;
using Dto.Plan;
using Entities;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Api.Seeds
{
    public static class DefaultPlansAndServices
    {


        public static async Task SeedAsync(DataContext context, IMapper mapper)
        {


            await context.PlanFeatures.ExecuteDeleteAsync();
            await context.Plans.ExecuteDeleteAsync();
            await context.SaveChangesAsync();


             await CreateModels(context, mapper);
            var planss = GetPlanCreateList(mapper);
            await context.Plans.AddRangeAsync(planss);
            foreach (var plan in planss)
            {
                foreach (var planFeature in plan.PlanFeatures)
                {
                    planFeature.PlanId= plan.Id;
                }
                await context.PlanFeatures.AddRangeAsync(plan.PlanFeatures);
            }

            
           

            await context.SaveChangesAsync();



        }







        

      static List<PlanFeatureCreate> GetPlanFeatureCreateFree()
            {





                var planFeatures = new List<PlanFeatureCreate>
                      {
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "AI Models" },
                                    { "ar", "عدد النماذج AI" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "3" },
                                    { "ar", "3" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "Requests" },
                                    { "ar", "الطلبات" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "1,000 requests" },
                                    { "ar", "1,000 طلب" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "Processor" },
                                    { "ar", "المعالج" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "Shared" },
                                    { "ar", "مشترك" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "RAM" },
                                    { "ar", "الذاكرة العشوائية" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "2 GB" },
                                    { "ar", "2 جيجابايت" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "Speed" },
                                    { "ar", "السرعة" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "2 pre/Second" },
                                    { "ar", "2 pre/Second" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "Support" },
                                    { "ar", "الدعم" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "No" },
                                    { "ar", "لا" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "Customization" },
                                    { "ar", "تخصيص" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "No" },
                                    { "ar", "لا" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "API" },
                                    { "ar", "API" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "No" },
                                    { "ar", "لا" }
                                }
                            },
                            new PlanFeatureCreate
                            {
                                Name = new Dictionary<string, string>
                                {
                                    { "en", "Space" },
                                    { "ar", "Space" }
                                },
                                Description = new Dictionary<string, string>
                                {
                                    { "en", "1" },
                                    { "ar", "1" }
                                }
                            }
                        };



                return planFeatures;

            }






        public static async Task   CreateModels(DataContext context, IMapper mapper)
        {

            var ListT2T=new List<ModelAiCreate>()

             {


                new ModelAiCreate()
                {
                    Type = "T2T",
                    Category = "Category1",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v2",
                    Name = "TextModel1",
                    Token = Guid.NewGuid().ToString()
                },

                  new ModelAiCreate()
                {
                    Type = "T2T",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard =false,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v1",
                    Name = "TextModel1",
                    Token = Guid.NewGuid().ToString()
                },


                  new ModelAiCreate()
                {
                    Type = "T2T",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v2",
                    Name = "TextModel1",
                    Token = Guid.NewGuid().ToString()
                },
                   new ModelAiCreate()
                {

                    Type = "T2T",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-M-v1",
                    Name = "TextModel2",
                    Token = Guid.NewGuid().ToString()
                },

                  new ModelAiCreate()
                {
                    Type = "T2T",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard =false,
                    AbsolutePath = "wasmdashai/vits-ar-sa-M-v2",
                    Name = "TextModel2",
                    Token = Guid.NewGuid().ToString()
                },

                  new ModelAiCreate()
                {
                    Type = "T2T",
                    Category = "News",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-M-v2",
                    Name = "TextModel2",
                    Token = Guid.NewGuid().ToString()
                },


            };
            var ListT2S = new List<ModelAiCreate>()

             {


                new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "News",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v2",
                    Name = "Heba",
                    Token = Guid.NewGuid().ToString()
                },

                  new ModelAiCreate()
                {

                    Type = "T2S",
                    Category = "News",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard =false,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v1",
                    Name = "Heba",
                    Token = Guid.NewGuid().ToString()
                },

                      new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Hejaz Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v2",
                    Name = "Heba",
                    Token = Guid.NewGuid().ToString()
                },


                  new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Hejaz Accent",
                    IsStandard =false,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v1",
                    Name = "TextModel1",
                    Token = Guid.NewGuid().ToString()
                },





                  new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "General",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-huba-v2",
                    Name = "TextModel1",
                    Token = Guid.NewGuid().ToString()
                },
                   new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "Category1",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-M-v1",
                    Name = "TextModel2",
                    Token = Guid.NewGuid().ToString()
                },

                  new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "Category1",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard =false,
                    AbsolutePath = "wasmdashai/vits-ar-sa-M-v2",
                    Name = "TextModel2",
                    Token = Guid.NewGuid().ToString()
                },

                  new ModelAiCreate()
                {
                    Type = "T2S",
                    Category = "Category2",
                    Language = "Arabic",
                    Dialect = "Najdi Accent",
                    IsStandard = true,
                    AbsolutePath = "wasmdashai/vits-ar-sa-M-v2",
                    Name = "TextModel2",
                    Token = Guid.NewGuid().ToString()
                },


            };



             var mT2T = mapper.Map<List<ModelAi>>(ListT2T);
             var mT2S = mapper.Map<List<ModelAi>>(ListT2S);

             await context.ModelAis.AddRangeAsync(mT2T);
             await context.SaveChangesAsync();



            await context.ModelAis.AddRangeAsync(mT2S);
            await context.SaveChangesAsync();

        }
  public static List<Plan> GetPlanCreateList(IMapper mapper)
        {








            var cplans = new List<PlanCreate>()
            {

               new PlanCreate()
               {
                   Name=new Dictionary<string, string>()
                   {
                                     { "en", "Free" },
                                    { "ar", "مجاني" }
                   }

                  ,
                   Description=new Dictionary<string, string>()
                   {
                       // حط الوصف 
                          { "en", "Free" },
                        { "ar", "مجاني" }
                   },
                   Price=0.0m,
                   Amount=0,
                   Features=GetPlanFeatureCreateFree()
               }
            };
            // ضيف بقية الخطط والمعلومات 


            var plans = mapper.Map<List<Plan>>( cplans );

            





            return plans;
        }

  private static Service[] GetServices(string modelAiId)
        {
            Service[] services = [
            new Service() { Name = "Text to text", Token = "bearer",AbsolutePath="t2t" ,ModelAiId=modelAiId},
                    new Service() { Name = "Audio",AbsolutePath="t2speech", Token = "bearer",ModelAiId=modelAiId },
                    new Service() { Name = "Speaker",AbsolutePath = "speaker", Token = "bearer" , ModelAiId = modelAiId},
                    ];

            return services;
        }

        private static List<Plan> GetPlans()
        {

            List<Plan> plans =
            [
                 new()
                {
                    Id = "price_1QSOh8KMQ7LabgRTu8QHKFJE",
                    BillingPeriod = "month",
                    //NumberRequests = 10,
                    ProductId = "prod_RL4cPSzDwjdQyh",
                    ProductName = "Free",
                    Description="DDDFGFGF",
                    Amount = 0,
                    Images=null,
                    Active=true,
                    UpdatedAt=DateTime.Today,
                    CreatedAt=DateTime.Today
                   
                   

        //public required long NumberRequests { get; set; }

 
                }
               

            ];

            return plans;

        }

    }
}