using BecomeAWizzard.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BecomeAWizzard.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var demo = new User { DisplayName = "Demo Wizard", Email = "demo@wizard.local" };
        demo.PasswordHash = new PasswordHasher<User>().HashPassword(demo, "Wizard123!");

        var quiz = new Quiz
        {
            Owner = demo,
            Title = "The Regression Dragon",
            Description = "Øv på enhetstesting, integrasjonstesting og regresjonstesting.",
            Topic = "Software Testing",
            IsPublished = true,
            BossFightEnabled = true,
            BossName = "Regression Dragon",
            BossHp = 300,
            BossDamagePerStreak = 100,
            Questions = new List<Question>
            {
                CreateQuestion(1, "Hva er hovedmålet med regresjonstesting?", "Regresjonstesting kontrollerer at eksisterende funksjonalitet fortsatt virker etter en endring.", 0,
                    "Kontrollere at endringer ikke har ødelagt eksisterende funksjoner", "Teste bare nye metoder", "Erstatte integrasjonstester", "Måle nettverkshastighet"),
                CreateQuestion(2, "Hva tester en enhetstest vanligvis?", "En enhetstest verifiserer en liten, isolert del av programmet.", 1,
                    "Hele produksjonssystemet", "En liten isolert kodeenhet", "Kun brukergrensesnittet", "Prosjektbudsjettet"),
                CreateQuestion(3, "Hva undersøker integrasjonstesting?", "Integrasjonstesting undersøker samspillet mellom flere komponenter.", 2,
                    "Fargevalg", "Individuelle variabler", "Samspillet mellom komponenter", "Kun dokumentasjon")
            }
        };

        db.Add(quiz);
        await db.SaveChangesAsync();
    }

    private static Question CreateQuestion(int order, string text, string explanation, int correctIndex, params string[] options) =>
        new()
        {
            Order = order,
            Text = text,
            Explanation = explanation,
            AnswerOptions = options.Select((option, index) => new AnswerOption
            {
                Text = option,
                IsCorrect = index == correctIndex
            }).ToList()
        };
}

