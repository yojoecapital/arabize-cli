I'm excited to announce the official release of the Arabize CLI.

## Features

To run the transliterator, provide a word sequence. Words should be separated by spaces, and letters should be separated by hyphens `-` or diacritics.

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

## Installation

To install the Arabize CLI on Linux, run the following command:

```bash
curl -L -o /tmp/arabize https://github.com/yojoecapital/arabize-cli/releases/latest/download/arabize && chmod 755 /tmp/arabize && sudo mv /tmp/arabize /usr/local/bin/
```
