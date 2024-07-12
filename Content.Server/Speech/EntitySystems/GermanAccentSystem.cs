using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed partial class GermanAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    [GeneratedRegex(@"(?<!\bthat)\bth", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex ThRegex();

    [GeneratedRegex(@"w", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex WRegex();

    private static readonly Dictionary<string, string> DirectReplacements = new()
    {
        { "and", "und" },
        { "yes", "ja" },
        { "no", "nein" },
        { "is", "ist" },
        { "please", "bitte" },
        { "thank you", "danke" },
        { "thanks", "danke" },
        { "hello", "hallo" },
        { "goodbye", "auf wiedersehen" },
        { "bye", "tschüss" },
        { "friend", "freund" },
        { "beer", "bier" },
        { "cheese", "käse" },
        { "doctor", "arzt" },
        { "food", "essen" },
        { "house", "haus" },
        { "school", "schule" },
        { "security", "polizei" },
        { "security officer", "polizeibeamter" },
        { "scientist", "wissenschaftler" },
        { "cargo", "fracht" },
        { "engineering", "technik" },
        { "chaplain", "kaplan" },
        { "captain", "kapitän" },
        { "passenger", "passagier" },
        { "shit", "scheiße" },
        { "fuck", "verdammt" },
        { "damn", "verdammt" },
        { "ass", "arsch" }
    };

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GermanAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    public string Accentuate(string message, GermanAccentComponent component)
    {
        // Order:
        // Do character manipulations first
        // Then direct word/phrase replacements
        // Then prefix/suffix

        var msg = message;

        // Character manipulations:
        // Replace all 'th' with 'z' (excluding 'that')
        msg = ThRegex().Replace(msg, "z");

        // Replace all 'w' with 'v'
        msg = WRegex().Replace(msg, "v");

        // Capitalize the first character if the message starts with 'z' or 'v' due to replacement
        if (!string.IsNullOrEmpty(msg) && (msg.StartsWith('z') || msg.StartsWith('v')))
        {
            msg = char.ToUpper(msg[0]) + msg.Substring(1);
        }

        // Direct word/phrase replacements:
        foreach (var (first, replace) in DirectReplacements)
        {
            // Capitalize if at the start of the message
            if (msg.StartsWith(first, StringComparison.OrdinalIgnoreCase))
            {
                var capitalizedReplace = char.ToUpper(replace[0]) + replace.Substring(1);
                msg = Regex.Replace(msg, $@"(?<!\w){first}(?!\w)", capitalizedReplace, RegexOptions.IgnoreCase);
            }
            else
            {
                msg = Regex.Replace(msg, $@"(?<!\w){first}(?!\w)", replace, RegexOptions.IgnoreCase);
            }
        }

        return msg;
    }

    private void OnAccentGet(EntityUid uid, GermanAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message, component);
    }
}
