using Microsoft.Extensions.Options;
using Stripe;
using Stripe.BillingPortal;

namespace StripeGateway
{
    public interface IStripeCustomer
    {
        Task<Customer> CreateAsync(CustomerCreateOptions createOptions);
        Task<CustomerSession> CreateCustomerSession(string customerId);
        Task<Session> CustomerPortal(Stripe.BillingPortal.SessionCreateOptions options);
        Task<Customer> GetCustomer(string customerId);
        Task<StripeList<Customer>> GetCustomersByEmail(string email);
    }

    public class StripeCustomer(IOptions<StripeOptions> _options) : IStripeCustomer
    {
        public async Task<Customer> GetCustomer(string customerId)
        {
            var service = new CustomerService();
            var customer = await service.GetAsync(customerId);
            //var result = mapper.Map<Customer>(customer);
            return customer;
        }

        public async Task<StripeList<Customer>> GetCustomersByEmail(string email)
        {
            var service = new CustomerService();

            Stripe.StripeList<Customer> customers = await service.ListAsync(new CustomerListOptions { Email = email });

            //var result = customers.Any() ? mapper.Map<Customer>(customers.First()) : null;
            return customers;
        }

        public async Task<CustomerSession> CreateCustomerSession(string customerId)
        {
            var options = new CustomerSessionCreateOptions
            {
                Customer = customerId,
                Components = new CustomerSessionComponentsOptions
                {
                    PricingTable = new CustomerSessionComponentsPricingTableOptions
                    {
                        Enabled = true,
                    },
                },
            };
            var service = new CustomerSessionService();
            var customer = await service.CreateAsync(options);
            return customer;
        }

        public async Task<Customer> CreateAsync(CustomerCreateOptions options)
        {
            var service = new CustomerService();
            var customer = await service.CreateAsync(options);
            //var customerResponse = mapper.Map<Customer>(customer);
            return customer;
        }
        public async Task<Session> CustomerPortal(Stripe.BillingPortal.SessionCreateOptions options)
        {
            // For demonstration purposes, we're using the Checkout session to retrieve the customer ID.
            // Typically this is stored alongside the authenticated user in your database.

            // Show plan customer for update or cancel
            //var checkoutService = new SessionService(this.client);
            //var checkoutSession = await checkoutService.GetAsync(sessionId);

            // This is the URL to which your customer will return after
            // they are done managing billing in the Customer Portal.
            //var returnUrl = _options.Value.ReturnUrl;

            //var options = new Stripe.BillingPortal.SessionCreateOptions
            //{
            //    Customer = customerId,
            //    ReturnUrl = returnUrl,
            //    //FlowData = new Stripe.BillingPortal.SessionFlowDataOptions
            //    //{
            //    //    Type = "subscription_update",
            //    //},
            //};
            //var service = new Stripe.BillingPortal.SessionService(this.client);
            var service = new Stripe.BillingPortal.SessionService();
            var session = await service.CreateAsync(options);
            //var result = mapper.Map<SessionResponse>(session);
            return session;
        }
    }
}
