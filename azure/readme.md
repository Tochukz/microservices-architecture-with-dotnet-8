# Azure Infrastructure 
### Description 
This connfiguration provisions all the Azure resources used for the Mango Microservices applications. 

### Setup 
#### Setup Azure 
__Install Azure CLI__  
Go to [install-azure-cli](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli) to install Azure CLI for your operating system.   

After Azure is install, _authenticate the CLI_ to be able to access your Azure subscription:  
```bat
> azure login
```  

You may be required to use you Azure Subscription, Tennat Id to login. 
On you Azure Portal Website, seach for _Microsoft Entra ID_ service and click on it.   
On the Overview page, the Tenant ID is displayed in the Basic information section. You can copy it directly.   
Use the tenant id to login 
```
>  azure login --tenant <your-tenant-id>
```


#### Setup Terraform
__Install Terraform__   
Install Terraform on Windows using _choco_.     
First open up ypur windows command prompt as an administrator.  
 
```bat
> choco install terraform
```

### Deployment 
Initialze the configuration, paln and apply. 
```bat
> terraform init
> terraform plan 
> terraform apply 
```

### After Deployment 
__Connection String (Azure Portal)__  
To get the connection string for the service bus from Azure Portal
* Go the Azure Portal -> _Service Bus_  service 
* Click on the service bus we created 
* Side navigation bar -> Settings -> Shared Access Policy 
* Under the Policy table, select the `RootManageSharedAccessKey`
* On the left modal,copy the _Primary connection string_

__Output__  
The `servicebus_namespace_connection_string` and the `queue_connection_string` output parameters are marked as sensitive and will be hidden it the output.  
To reveal these outputs, use the `-raw` flag 
```bat
> terraform output -raw servicebus_namespace_connection_string
> terraform output -raw queue_connection_string
```

__Usage__  
* Copy the `servicebus_namespace_connection_string` output value to be used for the `SERVICE_BUS_CONNECTION_STRING` in your `.env` file.  for the `Mango.Services.EmailAPI` and `Mango.Services.ShoppingCartAPI` projects.   
* Copy the queue name `emailshoppingcart` and `registration` too use in `appsettings.json` file.  