using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace XXTOLab4;

public class MessageService
{
    private readonly ITelegramBotClient _botClient;

    private readonly ReplyKeyboardMarkup _replyKeyboardMarkup = new(new[]
    {
        new KeyboardButton[]
            { MeesageConstans.Specialty, MeesageConstans.EducationalPrograms, MeesageConstans.MilitaryDepartment },
        new KeyboardButton[] { MeesageConstans.HowToGet, MeesageConstans.OfficialWebSite },
    })
    {
        ResizeKeyboard = true
    };

    private readonly CancellationTokenSource _cts;

    public MessageService(ITelegramBotClient botClient, CancellationTokenSource cts)
    {
        _botClient = botClient;
        _cts = cts;
    }
    
    public Task<Message> GetMessageFromInput(Message message)
        => message.Text! switch
        {
            MeesageConstans.HowToGet => _botClient.SendLocationAsync(
                chatId: message.Chat.Id,
                latitude: 49.83583333f,
                longitude: 24.01027778f,
                cancellationToken: _cts.Token),
            _ => DefaultMessage(message),
        };

    private Task<Message> DefaultMessage(Message message)
        => _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: GetResponseForMessage(message.Text!),
            replyMarkup: _replyKeyboardMarkup,
            cancellationToken: _cts.Token);
    
    private string GetResponseForMessage(string message)
        => message switch
        {
            MeesageConstans.Specialty => "0647 «Прикладна математика»",
            MeesageConstans.EducationalPrograms => "Освітні програми (спеціалізації)," +
                                                   " Прикладна математика та інформатика (бакалавр)," +
                                                   " Фінансовий інжиніринг (бакалавр)," +
                                                   " Прикладна математика (магістр)," +
                                                   " Інформаційно-комунікаційні технології (магістр)",
            MeesageConstans.MilitaryDepartment => "Присутня",
            MeesageConstans.HowToGet => "вулиця Митрополита Андрея, 5, Львів, Львівська область",
            MeesageConstans.OfficialWebSite => "https://amath.lp.edu.ua",
            _ => "Невідома операція, будь ласка виберіть один з варіантів з меню!"
        };
}