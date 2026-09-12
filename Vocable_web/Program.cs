using Vocable;
using Vocable.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Use session-backed TempData to avoid storing large JSON in cookies
var mvcBuilder = builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});
mvcBuilder.AddSessionStateTempDataProvider();

// create service to use serverside. 
AdverbsService adService = new AdverbsService();
adService.AddAnAdverbQuestionRepo();
GenericRepo<Adverb_Question> repo = new GenericRepo<Adverb_Question>();


builder.Services.AddSingleton<AdverbsService>(adService);
builder.Services.AddSingleton<Adverb_Question>();
builder.Services.AddSingleton<GenericRepo<Adverb_Question>>(repo);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
