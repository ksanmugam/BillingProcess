
# Billing Process

A simple service to accept a CDR file or CDR feed (a list of phone calls), match the data to a company, apply a relevant call rate and generate an invoice for each company with user and respective call charges.

## How to install
Please disregard below steps if already installed
****

- Download and install .NET Core v3.1
https://dotnet.microsoft.com/en-us/download/dotnet/3.1

**Disclaimer I had v3.1 already installed, limitations and exclusions for newer versions as it has not been tested**

*Yes yes, I know I know it's my responsibility (my bad)*

- Download and install Visual Studio Community (VS)
https://visualstudio.microsoft.com/downloads/
***

## Installation

1) Clone repository
- using [GitHub](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository?tool=webui) *(highly recommend SourceTree)*
- if unsure [ChatGPT](https://openai.com/blog/chatgpt/) would be able to assist you

2) Once cloned successfully, navigate to the downloaded folder path

3) Click on BillingProcess folder, you should see folder structure:
```
.
├── BillingProcess
├── .gitattributes
├── .gitignore
├── BillingProcess.sln
└── README.md
```

4) Go ahead and click on *BillingProcess.sln* and it should open VS program **if installed correctly*

5) On the navigation bar, select Build -> Build solution.

```bash
Build started...
1>------ Build started: Project: BillingProcess, Configuration: Debug Any CPU ------
Restored C:\Users\ksanmugam\source\repos\BillingProcess\BillingProcess\BillingProcess.csproj (in 258 ms).
1>C:\Users\ksanmugam\source\repos\BillingProcess\BillingProcess\Billing\DllApi\BillingApi.cs(29,52,29,72): warning CS1998: This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
1>C:\Users\ksanmugam\source\repos\BillingProcess\BillingProcess\Billing\DllApi\BillingApi.cs(48,52,48,79): warning CS1998: This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
1>BillingProcess -> C:\Users\ksanmugam\source\repos\BillingProcess\BillingProcess\bin\Debug\netcoreapp3.1\BillingProcess.dll
1>Done building project "BillingProcess.csproj".
========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========

```

6) If successful, you should see above in terminal, go ahead and press F5 to start debugging

7) Happy days

## Testing

To run tests, Go to [Postman](https://www.postman.com/) *create an account it's free* 

Postman API Platform would do *ensure you have installed [Postman Desktop Agent](https://www.postman.com/downloads/postman-agent/)*

Create a new workspace

#### Option 1
In the workspace, click on + to open a new request, select *GET* option, enter request URL as `https://localhost:<port>/api/billing`

Go to *Body*, click "raw", change Text to JSON on the dropdown list and enter schema as example below:
```bash
[
  {
    "carrierReference": 55683888429, // long
    "connectDateTime": "10/02/2023 4:37", // string
    "duration": 34, // int
    "sourceNumber": 61288555347, // long
    "destinationNumber": 61482960200, // long
    "direction": "OUTBOUND", // string
  }
]
```

#### Option 2
In the workspace, click on + to open a new request, select *POST* option, enter request URL as `https://localhost:<port>/api/billing/file`

Go to *Body*, click form-data, hover over "key" input field and find the hidden dropdown that says "Text". Click "Text", and then change it to say "File". In the "Key" field, enter word *file* in the input. In the "Value" field, click "Select file" and select the file to send.

*P.S. If you are having trouble uploading a file, check out this post [StackOverflow](https://stackoverflow.com/questions/60036239/upload-file-failed-postman)*

If all is well, click Send and you should receive your invoices.

If you're reached this far, give yourself a pat on the back! Well done! Look forward to working with you.


