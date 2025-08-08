using Application.Services.External.ComplianceService;
using Application.UseCases.AccountsUseCase.AddAccount;
using Application.UseCases.AccountsUseCase.AddCards;
using Application.UseCases.AccountsUseCase.AddInternalTransaction;
using Application.UseCases.AccountsUseCase.AddTransaction;
using Application.UseCases.AccountsUseCase.GetAccounts;
using Application.UseCases.AccountsUseCase.GetBalance;
using Application.UseCases.AccountsUseCase.GetCards;
using Application.UseCases.AccountsUseCase.GetTransactions;
using Application.UseCases.AccountsUseCase.RevertTransaction;
using Application.UseCases.CardsUseCase.GetCards;
using Application.UseCases.LoginUseCase.SignIn;
using Application.UseCases.PeopleUseCase.AddPeople;
using Domain.Interfaces.Base;
using Infrastructure.DataAccess.Sql.Bases;

namespace WebAppCubos.Modules
{
    public static class UseCaseExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<ISignInUseCase, SignInUseCase>();
            services.AddScoped<IAddPeopleUseCase, AddPeopleUseCase>();
            services.AddScoped<IComplianceService, ComplianceService>();
            services.AddScoped<IGetAccountsUseCase, GetAccountsUseCase>();
            services.AddScoped<IAddAccountUseCase, AddAccountUseCase>();
            services.AddScoped<IAddCardUseCase, AddCardUseCase>();
            services.AddScoped<IGetCardsUseCase, GetCardsUseCase>();
            services.AddScoped<IGetUserCardsUseCase, GetUserCardsUseCase>();
            services.AddScoped<IAddInternalTransactionUseCase, AddInternalTransactionUseCase>();
            services.AddScoped<IAddTransactionUseCase, AddTransactionUseCase>();
            services.AddScoped<IGetBalanceUseCase, GetBalanceUseCase>();
            services.AddScoped<IGetTransactionsUseCase, GetTransactionsUseCase>();
            services.AddScoped<IRevertTransactionUseCase, RevertTransactionUseCase>();

            return services;
        }
    }
}