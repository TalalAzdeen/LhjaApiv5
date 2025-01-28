using AutoMapper;
using Dto.Stripe.CustomerDto;
using Dto.Stripe.Product;
using Dto.Stripe.Session;
using Dto.Stripe.StripeSubscription;
using Stripe;
using Stripe.Checkout;
using StripeGateway;

namespace Api.Config
{
    public class CurrencyFormatter : IValueConverter<long, decimal>
    {
        public decimal Convert(long source, ResolutionContext context)
        {
            decimal d = source / 100m;
            return d;
        }
    }


    public class StripeMappingConfig : Profile
    {
        public StripeMappingConfig()
        {
            CreateMap<Price, PriceResponse>()
                .ForMember(p => p.Interval, pp => pp.MapFrom(p => p.Recurring.Interval))
                .ForMember(p => p.UnitAmountDecimal, opt => opt.ConvertUsing(new CurrencyFormatter(), "UnitAmount"))
                .ReverseMap();

            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Customer, CustomerResponse>().ReverseMap();
            CreateMap<Session, SessionResponse>().ReverseMap();
            CreateMap<Stripe.BillingPortal.Session, SessionResponse>().ReverseMap();
            CreateMap<Subscription, StripeSubscriptionResponse>().ReverseMap();


            CreateMap<ProductCreate, ProductCreateOptions>();
            CreateMap<ProductUpdate, ProductUpdateOptions>();


        }
    }
}