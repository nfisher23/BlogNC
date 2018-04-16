# [BlogNC](http://blognc.org)
BlogNC is an open source blogging framework, whose architecture is based on the ASP .NET Core MVC technology. As such, it can be deployed to any platform that .NET Core can be deployed to (which is, well, a lot of platforms).
## Usage
Currently, the best way to use this framework is to clone it locally, then to make design adjustments as you see fit before pushing to a production server:
```
git clone https://github.com/nfisher23/BlogNC.git
```
It's always a good idea to run the unit tests afterwards:
```
cd BlogNC/BlogNC.UnitTests
dotnet test
```
You should see every test pass.

### Current Features
This version (the first) of BlogNC includes prebuilt sidebar widgets for category sorting, archives, and recent posts. The administration page lets you add and manage static pages, specifying what priority (order with 0 being the highest/soonest) and whether or not to include a link in the nav-bar or the footer.

It also lets you create drafts, then preview how that draft will look before publishing it to a post. You can simultaneously keep the preview page open, then refresh it as you click the "update draft" button to dynamically see changes as continue to march forward.
### Deployment
After you make the asthetic changes you want, you can deploy to a server of your choosing. Some notes on that: 
- Make sure that you set the ASPNETCORE_ENVIRONMENT environment variable equal to Production. The development environment uses an in-memory database, so stopping and starting the application will cause you to lose any data.
- It is recommended that you get a SSL Certificate, since you will have to log in to the application (username and password) to make posts, drafts, and static pages.
- The current version is configured to use SQLite for data storage. This is probably fine if all you want to do is get a working blog up, but if you plan on changing anything, ever, or want the option of making improvements and additions, we recommend changing the configuration to connect to a different database. SQLite database migrations are extremely limited. Postgre is probably our favorite.

## After deployment Usage
Once it is deployed, navigate to /NCAdmin in your URL browser. You will be redirected to /Account/Login. The default seed data is:
- username: "BlogNCUser"
- password: "BlogNCDefaultPassword1!"

If you elect to keep this default configuration before deployment, you can enter those credentials to gain access to the application after you try to access /NCAdmin. Then you will be redirected to /Account/CreateAccount. Enter your username and password to create your own "account." This will destroy the default seed data above and your new information will be how you access the application in the future.
From there, you can add static pages, create posts, and make work-in-progress drafts. Once you get here, the sky is basically the limit.

## Contributing
This project is really just getting started. What you see here truly is a Minimum Viable Release. If you want to contribute, please do submit a pull request. What we really want more than anything right now are great web designers to provide nice looking templates, rather than the base one that is included (which, we believe, is incredibly average). If you want to make feature additions, off the top of my head we are interested in:
- Support for file and image uploading, as well as access to files from within the application (like wordpress in that regard, basically). 
- The ability to add and remove widgets from the sidebar, and other fun pages (like recent posts pages).
- A user friendly way to adjust things like fonts, color styling, page titles
- An ability to select multiple page stylings
- Javascript support for many features, without going overboard (everything works right now, after all).

If you do decide to contribute features, follow interfaced-based design principles and write unit tests for all the features you implement. If you break any of the existing unit tests or if you create features that are not properly tested, your code will not be merged, so do ensure that your code passes them.

While there are many optimizations and performance improvements that can be made, we will deliberately not optimize many things like database queries through EF Core just yet.

We are working on automating build procedures for this app. Please stay tuned for intuitive and simple deployment and upgrades.
