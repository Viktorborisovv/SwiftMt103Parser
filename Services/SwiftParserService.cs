using SwiftMt103Parser.Api.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SwiftMt103Parser.Api.Services
{
    public class SwiftParserService
    {
        public SwiftMessage Parse(string rawMessage)
        {
            SwiftMessage message = new SwiftMessage
            {
                RawMessage = rawMessage,
                TransactionReference = GetFieldValue(rawMessage, "20"),
                BankOperationCode = GetFieldValue(rawMessage, "23B"),
                OrderingCustomer = GetFieldValue(rawMessage, "50K"),
                BeneficiaryCustomer = GetFieldValue(rawMessage, "59"),
                PaymentDetails = GetFieldValue(rawMessage, "70"),
                DetailsOfCharges = GetFieldValue(rawMessage, "71A"),
                CreatedOn = DateTime.UtcNow
            };

            string? field32A = GetFieldValue(rawMessage, "32A");

            if (!string.IsNullOrWhiteSpace(field32A))
            {
                ParseField32A(field32A, message);
            }

            return message;
        }

        private static string? GetFieldValue(string rawMessage, string fieldName)
        {
            string pattern = $@":{fieldName}:(.*?)(?=\r?\n:\d{{2}}[A-Z]?:|\r?\n-}}|$)";

            Match match = Regex.Match(
                rawMessage,
                pattern,
                RegexOptions.Singleline
            );

            if(!match.Success)
            {
                return null;
            }

            return match.Groups[1].Value.Trim();
        }

        private static void ParseField32A(string field32A, SwiftMessage message)
        {
            if(field32A.Length < 10)
            {
                return;
            }

            string valueDateRaw = field32A.Substring(0, 6);
            string currency = field32A.Substring(6, 3);
            string amountRaw = field32A.Substring(9);

            message.ValueDate = FormatValueDate(valueDateRaw);
            message.Currency = currency;

            amountRaw = amountRaw.Replace(",", ".");

            if(decimal.TryParse(amountRaw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount))
            {
                message.Amount = amount;
            }
        }

        private static string FormatValueDate(string valueDateRaw)
        {
            if(DateTime.TryParseExact(
                valueDateRaw,
                "yyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date))
            {
                return date.ToString("yyyy-MM-dd");
            }

            return valueDateRaw;
        }
    }
}
