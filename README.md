# ECommerceNet8



## ASP.NET Core .NET 8 E-Commerce Backend



**ECommerceNet8** is a backend-focused e-commerce API built with **ASP.NET Core .NET 8**, **Entity Framework Core**, **SQL Server**, **JWT authentication**, **Redis caching**, **Stripe payment integration**, and mailing services. The project is structured as a multi-layer solution with separate API, Core, and Infrastructure projects, making it a strong portfolio example for clean backend design and real-world commerce workflows.



> The main purpose of this project is to demonstrate how a production-style e-commerce backend can be organized around clear responsibilities, reusable services, and secure API endpoints.
> 


## Technical Overview



| Area | Implementation |

|---|---|

| Runtime | .NET 8 |

| API Framework | ASP.NET Core Web API |

| Architecture | Layered API, Core, and Infrastructure projects |

| Data Access | Entity Framework Core with SQL Server |

| Authentication | JWT Bearer authentication and ASP.NET Core Identity packages |

| Caching | StackExchange Redis and Redis distributed cache support |

| Payments | Stripe.net integration |

| Mail | MailKit and MimeKit |

| API Documentation | Swagger / OpenAPI |

| Extra Services | PDF-related packages and static file support |



## Main Features



The API includes modules for products, product variants, sizes, colors, materials, categories, shopping carts, orders, addresses, authentication, and payments. These features make the project suitable for demonstrating domain modeling, service-oriented backend development, and API-first e-commerce design.



| Feature Group | Example Controllers or Capabilities |

|---|---|

| Catalog | Products, main categories, variants, colors, sizes, and materials |

| Customer Workflow | Address management and shopping cart operations |

| Ordering | Order creation and order-related workflow endpoints |

| Security | Authentication endpoints and JWT-based request protection |

| Payment | Payment controller prepared for Stripe integration |

| Infrastructure | SQL Server persistence, Redis cache, mailing, and Swagger documentation |



## Solution Structure



```text

ECommerceNet8/

├── ECommerceNet8.Api/             # ASP.NET Core API, controllers, configuration, and startup

├── ECommerceNet8.Core/            # Core domain contracts, entities, DTOs, and business abstractions

├── ECommerceNet8.Infrastructure/  # Data access, external services, and infrastructure implementations

└── ECommerceNet8.sln              # Visual Studio solution

```



## Getting Started



Clone the repository and restore the .NET dependencies before running the API project.



```bash

git clone https://github.com/MuhammedWaly/ECommerceNet8.git

cd ECommerceNet8

dotnet restore

dotnet build

```



Update the connection strings, JWT settings, Stripe settings, Redis connection, and mail settings in `ECommerceNet8.Api/appsettings.json` or `appsettings.Development.json` according to your local environment. After configuration, run the API project.



```bash

dotnet run --project ECommerceNet8.Api

```



When the application starts successfully, open the Swagger endpoint in the browser to explore and test the available API endpoints.



```text

https://localhost:<port>/swagger

```



## Configuration Notes



This repository is intended as a portfolio and learning project. Before deploying it to a public environment, move secrets such as JWT keys, mail credentials, Stripe keys, and da
