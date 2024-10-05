# Arabize

This project is a simple Arabic transliteration tool that converts ASCII input into Arabic script with the appropriate diacritics. 


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

## Building
To compile the project and run it as a self-contained executable:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
```

This will generate a single executable file. Replace `linux-x64` with whatever OS your using.