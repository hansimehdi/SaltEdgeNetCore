using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaltEdgeNetCore.Models.Account;
using SaltEdgeNetCore.Models.Assets;
using SaltEdgeNetCore.Models.Attempts;
using SaltEdgeNetCore.Models.Category;
using SaltEdgeNetCore.Models.Connections;
using SaltEdgeNetCore.Models.ConnectSession;
using SaltEdgeNetCore.Models.Consents;
using SaltEdgeNetCore.Models.Country;
using SaltEdgeNetCore.Models.Currencies;
using SaltEdgeNetCore.Models.Customer;
using SaltEdgeNetCore.Models.HolderInfo;
using SaltEdgeNetCore.Models.Merchant;
using SaltEdgeNetCore.Models.OAuthProvider;
using SaltEdgeNetCore.Models.Provider;
using SaltEdgeNetCore.Models.Rates;
using SaltEdgeNetCore.Models.Responses;
using SaltEdgeNetCore.Models.Transaction;

namespace SaltEdgeNetCore.Client
{
    public interface ISaltEdgeClientV5
    {
        /// <summary>
        /// The country is represented just as a string. We’re using ISO 3166-1 alpha-2 country codes.
        /// Thus, all the country codes will have exactly two uppercase letters. There are two special cases:
        /// “Other”, encoded as XO
        /// “Fake”, encoded as XF
        /// </summary>
        /// <returns>Returns a list of countries supported by Salt Edge API.</returns>
        Task<IEnumerable<SeCountry>> ListCountriesAsync();

        /// <summary>
        /// Provider show allows you to inspect the single provider in order to give your users a proper
        /// interface to input their credentials.
        /// </summary>
        /// <param name="providerCode"></param>
        /// <returns>Provider instance</returns>
        Task<SeProvider> ProviderShowAsync(string providerCode);

        /// <summary>
        /// Returns all the providers we operate with. If a provider becomes disabled, it is not included in the list.
        /// </summary>
        /// <param name="fromDate">
        /// filtering providers created or updated starting from this date, defaults to null
        /// <remarks>date format should be "yyyy-mm-dd"</remarks>
        /// </param>
        /// <param name="fromId">the id of the provider which the list starts with, defaults to null</param>
        /// <param name="countryCode">filtering providers by country, defaults to null</param>
        /// <param name="mode">filtering providers by mode, possible values are: oauth, web, api, file</param>
        /// <param name="includeFakeProviders">whether you wish to fetch the fake providers, defaults to false</param>
        /// <param name="includeProviderFields">whether you wish to include all provider fields in the provider objects, defaults to false</param>
        /// <param name="providerKeyOwner">filtering providers by key owner, possible values are: client, saltedge. When value is set as client,
        /// only providers with client-set keys will be returned.</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SeProvider>, SePaging>> ProvidersListAsync(DateTime? fromDate = null,
            string fromId = default,
            string countryCode = default,
            string mode = default,
            bool includeFakeProviders = false,
            bool includeProviderFields = false,
            string providerKeyOwner = default);

        /// <summary>
        /// Creates a customer
        /// </summary>
        /// <param name="customerId">a unique identifier of the new customer</param>
        /// <returns>Customer instance</returns>
        Task<SeCustomer> CreateCustomerAsync(string customerId);

        /// <summary>
        /// Get a customer
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>Customer instance</returns>
        Task<SeCustomer> CustomerShowAsync(string customerId);

        /// <summary>
        /// List all of your app’s customers.
        /// </summary>
        /// <param name="fromId"> Starting from id</param>
        /// <param name="nextId"> Next id fetch</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SeCustomer>, SePaging>> CustomersListAsync(string fromId = default,
            string nextId = default);

        /// <summary>
        /// Remove a customer
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>RemoveCustomer instance</returns>
        Task<RemoveCustomer> CustomerRemoveAsync(string customerId);

        /// <summary>
        /// Lock a customer and its data 
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>LockCustomer instance</returns>
        Task<LockCustomer> CustomerLockAsync(string customerId);

        /// <summary>
        /// Unlock a customer and its data
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>UnlockCustomer instance</returns>
        Task<UnlockCustomer> CustomerUnlockAsync(string customerId);

        /// <summary>
        /// Allows to create a connect session, which will be used to access Salt Edge Connect for connection creation.
        /// </summary>
        /// <param name="session">CreateSession object</param>
        /// <returns>SessionResponse instance</returns>
        Task<SessionResponse> SessionCreateAsync(CreateSession session);

        /// <summary>
        /// Allows to reconnect session, which will be used to access Salt Edge Connect to reconnect a connection.
        /// </summary>
        /// <param name="session">ReconnectSession object</param>
        /// <returns>SessionResponse instance</returns>
        Task<SessionResponse> SessionReconnectAsync(ReconnectSession session);

        /// <summary>
        /// Allows refresh a session, which will be used to access Salt Edge Connect to refresh a connection.
        /// </summary>
        /// <param name="session">RefreshSession object</param>
        /// <returns>SessionResponse instance</returns>
        Task<SessionResponse> SessionRefreshAsync(RefreshSession session);

        /// <summary>
        /// create a connection for an OAuth provider
        /// </summary>
        /// <param name="oAuthProvider">CreateOAuthProvider object</param>
        /// <returns>OAuthProviderResponse instance</returns>
        Task<OAuthProviderResponse> OAuthProviderCreateAsync(CreateOAuthProvider oAuthProvider);

        /// <summary>
        /// reconnect a connection for an OAuth provider
        /// </summary>
        /// <param name="oAuthProvider">ReconnectOAuthProvider object</param>
        /// <returns>OAuthProviderResponse instance</returns>
        Task<OAuthProviderResponse> OAuthProviderReconnectAsync(ReconnectOAuthProvider oAuthProvider);

        /// <summary>
        /// authorize a connection for an OAuth provider when using client owned provider keys.
        /// </summary>
        /// <param name="oAuthProvider">AuthorizeOAuthProvider object</param>
        /// <returns>AuthorizeOAuthProviderResponse instance</returns>
        Task<AuthorizeOAuthProviderResponse> AuthProviderAuthorizeAsync(AuthorizeOAuthProvider oAuthProvider);

        /// <summary>
        /// List all the connections accessible to your application for certain customer.
        /// </summary>
        /// <param name="customerId">The id of the customer</param>
        /// <param name="fromId">Fetch from id</param>
        /// <returns></returns>
        Task<Response<IEnumerable<SeConnection>, SePaging>> ConnectionsListAsync(string customerId,
            string fromId = default);

        /// <summary>
        /// Get a connection
        /// </summary>
        /// <param name="connectionId">The id of the connection</param>
        /// <returns>Connection instance</returns>
        Task<SeConnection> ConnectionShowAsync(string connectionId);

        /// <summary>
        /// Removes a connection from our system and revokes the consent.
        /// </summary>
        /// <param name="connectionId">The id of the connection</param>
        /// <returns>RemoveConnection instance</returns>
        Task<RemoveConnection> ConnectionRemoveAsync(string connectionId);

        /// <summary>
        /// Get essential information about an account holder.
        /// </summary>
        /// <param name="connectionId">The id of the connection</param>
        /// <returns>HolderInfo instance</returns>
        Task<SeHolderInfo> HolderInfoShowAsync(string connectionId);

        /// <summary>
        ///  List of all attempts for a certain connection.
        /// </summary>
        /// <param name="connectionId">The id of the connection whose attempts are to be fetched</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SeAttempt>, SePaging>> AttemptsListAsync(string connectionId);

        /// <summary>
        /// Show attempt for a certain connection
        /// </summary>
        /// <param name="connectionId">The id of the connection whose attempt is to be fetched</param>
        /// <param name="attemptId">The attempt id</param>
        /// <returns>Attempt instance</returns>
        Task<SeAttempt> AttemptShowAsync(string connectionId, string attemptId);

        /// <summary>
        /// list of accounts of a connection to a related customer
        /// </summary>
        /// <param name="connectionId">The id of the connection whose attempts are to be fetched</param>
        /// <param name="customerId">
        /// The id of the customer containing the accounts, required unless connection_id parameter is sent.
        /// <remarks>Will be ignored if connection_id is present.</remarks>
        /// </param>
        /// <param name="fromId">The id of the account which the list starts with</param>
        /// <returns></returns>
        Task<Response<IEnumerable<SeAccount>, SePaging>> AccountListAsync(string connectionId,
            string customerId = default,
            string fromId = default);

        /// <summary>
        /// List of non duplicated transactions of an account.
        /// </summary>
        /// <param name="connectionId">The id of the connection</param>
        /// <param name="accountId">The id of the account</param>
        /// <param name="fromId">The id of the transaction which the list starts with</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SaltEdgeTransaction>, SePaging>> TransactionsListAsync(string connectionId,
            string accountId = default, string fromId = default);

        /// <summary>
        /// List all duplicated transactions of an account.
        /// </summary>
        /// <param name="connectionId">The id of the connection</param>
        /// <param name="accountId">The id of the account</param>
        /// <param name="fromId">The id of the transaction which the list starts with</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SaltEdgeTransaction>, SePaging>> TransactionsDuplicatedListAsync(string connectionId,
            string accountId = default, string fromId = default);

        /// <summary>
        /// List all pending transactions of an account.
        /// </summary>
        /// <param name="connectionId">The id of the connection</param>
        /// <param name="accountId">The id of the account</param>
        /// <param name="fromId">The id of the transaction which the list starts with</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SaltEdgeTransaction>, SePaging>> TransactionsPendingListAsync(string connectionId,
            string accountId = default,
            string fromId = default);

        /// <summary>
        /// Mark a list of transactions as duplicated.
        /// </summary>
        /// <param name="customerId">The id of the customer</param>
        /// <param name="transactionIds">The list of transactions id‘s</param>
        /// <returns>DuplicatedResponse instance</returns>
        Task<DuplicatedResponse> TransactionsDuplicateAsync(string customerId, IEnumerable<string> transactionIds);

        /// <summary>
        /// Remove duplicated flag from a list of transactions.
        /// </summary>
        /// <param name="customerId">The id of the customer</param>
        /// <param name="transactionIds">The list of transactions id‘s</param>
        /// <returns>UnDuplicatedResponse instance</returns>
        Task<UnDuplicatedResponse> TransactionsUnDuplicateAsync(string customerId, IEnumerable<string> transactionIds);

        /// <summary>
        /// Remove transactions older than a specified period.
        /// </summary>
        /// <param name="customerId">The id of the customer</param>
        /// <param name="accountId">The id of the account</param>
        /// <param name="keepDays">The amount of days for which the transactions will be kept.</param>
        /// <returns></returns>
        Task<RemoveTransactionResponse> TransactionRemoveAsync(string customerId, string accountId, int keepDays = 0);

        /// <summary>
        /// List all the consents accessible to your application for certain customer or connection.
        /// </summary>
        /// <param name="connectionId">The id of the connection containing the consents</param>
        /// <param name="customerId">
        /// The id of the customer containing the consents.
        /// <remarks>Will be ignored if connection_id is present.</remarks>
        /// </param>
        /// <param name="fromId">The id from which the next page of consents starts</param>
        /// <returns>Salt Edge Response instance</returns>
        Task<Response<IEnumerable<SeConsent>, SePaging>> ConsentsListAsync(string connectionId = default,
            string customerId = default,
            string fromId = default);

        /// <summary>
        /// Show a consent
        /// </summary>
        /// <param name="id">The id of the consent</param>
        /// <param name="connectionId">the id of the connection containing the consent,
        /// required unless customer_id parameter is not null</param>
        /// <param name="customerId">the id of the customer containing the consent,
        /// required unless connection_id parameter is not null</param>
        /// <returns>Consent instance</returns>
        Task<SeConsent> ConsentShowAsync(string id, string connectionId = default, string customerId = default);

        /// <summary>
        /// Revoke a consent
        /// </summary>
        /// <param name="id">The id of the consent</param>
        /// <param name="connectionId">the id of the connection containing the consent,
        /// required unless customer_id parameter is not null</param>
        /// <param name="customerId">the id of the customer containing the consent,
        /// required unless connection_id parameter is not null</param>
        /// <returns>Consent instance</returns>
        Task<SeConsent> ConsentRevokeAsync(string id, string connectionId = default, string customerId = default);

        /// <summary>
        /// The list of all the categories
        /// </summary>
        /// <returns>An object instance structured as parent and child</returns>
        Task<IDictionary<string, IEnumerable<string>>> CategoryListAsync();

        /// <summary>
        /// Change the category of some transactions, thus improving the categorization accuracy.
        /// </summary>
        /// <param name="customerId">The id of the customer</param>
        /// <param name="transactionsList">A list of category learn object</param>
        /// <returns>CategoryLearnResponse instance</returns>
        Task<CategoryLearnResponse> CategoryLearnAsync(string customerId, IEnumerable<SeCategoryLearn> transactionsList);

        /// <summary>
        /// The list of all the currencies
        /// </summary>
        /// <returns>A list of Currency object</returns>
        Task<IEnumerable<SeCurrency>> CurrenciesAsync();

        /// <summary>
        /// The list of all the assets 
        /// </summary>
        /// <returns>A list of Asset object</returns>
        Task<IEnumerable<SeAsset>> AssetsAsync();

        /// <summary>
        /// The list of all the currency rates
        /// </summary>
        /// <param name="date">Date for which currency rates will be retrieved</param>
        /// <returns>A list of Rates</returns>
        Task<IEnumerable<SeRate>> RatesAsync(DateTime? date = null);

        /// <summary>
        /// The list of a merchants information
        /// </summary>
        /// <param name="merchantIds">The list of the merchant id's to be fetched</param>
        /// <returns>A list of Merchant object</returns>
        Task<IEnumerable<Merchant>> MerchantsListAsync(IEnumerable<string> merchantIds);

        /// <summary>
        /// Show a merchant information
        /// </summary>
        /// <param name="merchantId">The merchant id to be fetched</param>
        /// <returns>Merchant instance</returns>
        Task<Merchant> MerchantShowAsync(string merchantId);
    }
}