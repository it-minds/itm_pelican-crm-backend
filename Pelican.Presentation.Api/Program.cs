using Pelican.Application;
using Pelican.Infrastructure.Authentication;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistince(builder.Configuration);
builder.Services.AddApplication();
//builder.Services.AddAzure();
builder.Services.AddAuth(builder.Configuration);



//Adding GraphQl Specific extensions and services.
var executorBuilder = builder.Services
	.AddPresentationGraphQL()
	.AddDataLoaders();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseCookiePolicy();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
	endpoints.MapGraphQL();
	endpoints.MapControllers();

});



app.Run();




