# ArabizeCli

This project is a simple Arabic transliteration tool that converts ASCII input into Arabic script with the appropriate diacritics. 

## Installation

You can execute the following command to install or update `arabize`.

```bash
curl -L -o /tmp/arabize https://github.com/yojoecapital/arabize/releases/latest/download/arabize && chmod 755 /tmp/arabize && sudo mv /tmp/arabize /usr/local/bin/
```

## Usage

To run the transliterator, provide a word sequence. Words should be separated by spaces, and letters should be separated by hyphens (`-`) or diacritics.

```bash
arabize ya%waw-seen%fa
# The output will be: يُوسُف
```

### Help Command
Use the `--help` or `-h` argument to see all supported letter and diacritic combinations:

```bash
arabize --help
```

### Macros

You can define your own shortcut replacements by making a JSON file at `~/.config/arabize/macros.json`. And example JSON file looks like this:

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