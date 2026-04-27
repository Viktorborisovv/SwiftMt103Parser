namespace SwiftMt103Parser.Api.Services
{
    using SwiftMt103Parser.Api.Data;
    using SwiftMt103Parser.Api.DTOs;
    using SwiftMt103Parser.Api.Models;

    public class SwiftMessageService
    {
        private readonly SwiftParserService parserService;
        private readonly SwiftMessageRepository repository;
        private readonly ILogger<SwiftMessageService> logger;

        public SwiftMessageService(SwiftParserService parserService, SwiftMessageRepository repository, ILogger<SwiftMessageService> logger)
        {
            this.parserService = parserService;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<SwiftMessageResponse> CreateFromTextAsync(string rawMessage)
        {
            logger.LogInformation("Started processing MT103 message.");

            SwiftMessage message = parserService.Parse(rawMessage);

            int id = await repository.AddAsync(message);

            message.Id = id;

            logger.LogInformation(
                "MT103 message saved successfully. Id: {Id}, TransactionReference: {TransactionReference}",
                message.Id,
                message.TransactionReference);

            return MapToResponse(message);
        }

        public async Task<List<SwiftMessageResponse>> GetAllAsync()
        {
            logger.LogInformation("Getting all saved MT103 messages.");

            List<SwiftMessage> messages = await repository.GetAllAsync();

            return messages
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<SwiftMessageResponse?> GetByIdAsync(int id)
        {
            logger.LogInformation("Getting MT103 message by Id: {Id}", id);

            SwiftMessage? message = await repository.GetByIdAsync(id);
            
            if (message == null)
            {
                logger.LogWarning("MT103 message with Id {Id} was not found.", id);
                return null;
            }

            return MapToResponse(message);
        }

        private static SwiftMessageResponse MapToResponse(SwiftMessage message)
        {
            return new SwiftMessageResponse
            {
                Id = message.Id,
                TransactionReference = message.TransactionReference,
                BankOperationCode = message.BankOperationCode,
                ValueDate = message.ValueDate,
                Currency = message.Currency,
                Amount = message.Amount,
                OrderingCustomer = message.OrderingCustomer,
                BeneficiaryCustomer = message.BeneficiaryCustomer,
                PaymentDetails = message.PaymentDetails,
                DetailsOfCharges = message.DetailsOfCharges,
                CreatedOn = message.CreatedOn
            };
        }
    }
}
