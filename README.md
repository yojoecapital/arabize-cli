# ArabizeCli

This project is a simple Arabic transliteration tool that converts ASCII input into Arabic script with the appropriate diacritics. 

## Installation

You can execute the following command to install or update `arabize`.

```bash
curl -L -o /tmp/arabize https://github.com/yojoecapital/arabize/releases/latest/download/arabize-cli && chmod 755 /tmp/arabize && sudo mv /tmp/arabize /usr/local/bin/
```

## Usage

To run the transliterator, provide a word sequence. Words should be separated by spaces, and letters should be separated by hyphens (`-`) or diacritics.

```bash
arabize ya%waw-seen%fa
# The output will be: يُوسُف
```

### List mappings

- use `list l` to list letter mappings
- use `list d` to list diacritic mappings
- use `list m` to list custom macro mappings

### Macros

You can use the `edit` command to open the macros file in your default text editor.

```json
{
	"yusuf": "ya%waw-seen%fa",
	"al": "alif-lam"
}
```

Now, `arabize` will perform the following transliteration:

```bash
arabize yusuf
# The output will be: يُوسُف
```

**Make sure** not to include hyphens (-) or space characters in your macros!

## Building

To compile the project and run it as a self-contained executable:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
```

This will generate a single executable file. Replace `linux-x64` with whatever OS your using.