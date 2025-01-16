using Microsoft.EntityFrameworkCore;

namespace bibliotecaPDF.Context;

public static class DbMigrationConfig
{
    public static void ConfigureInitialMigration(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        try{
            using ApplicationDbContext? context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context?.Database.Migrate();
            Console.WriteLine("Migração feita com sucesso");
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
        
    }
}