namespace ENTOS.Module.Services
{
    public partial class EntryFolderService
    {
        private static decimal GetAccountEntryContribution(AccountEntry accountEntry, EntryType folderEntryType, string choice)
        {
            if (accountEntry.Amount is null)
                return 0;
            if (choice.StartsWith("Book1") && accountEntry.Book1 == false)
                return 0;
            if (choice.StartsWith("Book2") && accountEntry.Book2 == false)
                return 0;

            var entryTemp = accountEntry.Amount.Value;

            if (choice == "Book1Sum" || choice == "Book2Sum")
            {
                bool mark = (accountEntry.Debit && folderEntryType == EntryType.Debit) || (!accountEntry.Debit && folderEntryType != EntryType.Debit);
                return mark ? entryTemp : -entryTemp;
            }
            else if (choice == "Book1Debit" || choice == "Book2Debit")
            {
                return accountEntry.Debit ? entryTemp : 0;
            }
            else if (choice == "Book1Credit" || choice == "Book2Credit")
            {
                return !accountEntry.Debit ? entryTemp : 0;
            }

            return 0;
        }

        private static decimal GetPartyAccountContribution(PartyAccount partyAccount, string choice)
        {
            if (partyAccount.Amount is null)
                return 0;
            if (choice.StartsWith("Book1") && partyAccount.Book1 == false)
                return 0;
            if (choice.StartsWith("Book2") && partyAccount.Book2 == false)
                return 0;

            var partyTemp = partyAccount.Amount.Value;

            if (choice == "Book1Sum" || choice == "Book2Sum")
            {
                bool mark = (partyAccount.Debit && partyAccount.EntryFolder.EntryType != EntryType.Debit) || (!partyAccount.Debit && partyAccount.EntryFolder.EntryType == EntryType.Debit);
                return mark ? partyTemp : -partyTemp;
            }
            else if (choice == "Book1Debit" || choice == "Book2Debit")
            {
                return !partyAccount.Debit ? partyTemp : 0;
            }
            else if (choice == "Book1Credit" || choice == "Book2Credit")
            {
                return partyAccount.Debit ? partyTemp : 0;
            }

            return 0;
        }
    }
}
