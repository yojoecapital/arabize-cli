# Arabize

- This is a command for Windows Systems to translate Arabic-transliterated letters into Arabic Unicode characters and copy to clipboard. 
- To access it easier, place the path of the `Arabize` directory in user's `Path` Environment Variable.
- Arabize can either be used as an interactive Interpreter (REPL) or a command-line program. To begin the REPL, just execute `arabize.exe` without any arguments.

## Usage

Use `arabize letters` to get a list of all transliterated letters and their Unicode mappings.

Pass the transliterated letters to have the Arabic Unicode copied to clipboard. For example:

```
> alif_lam_ra_ha_meem_alif_noon
```

This copies this text:

> الرحمان 

Use `_` characters to delimit letters. Use space characters to delimit words. For example:

```
> ba_seen_meem alif_lam_lam_haa alif_lam_ra_ha_meem_alif_noon alif_lam_ra_ha_ya_meem
```

This copies this text:

> بسم الله الرحمان الرحيم

### How to Use Diacritics

Use these sequences at the end of a transliterated letter to add a diacritic ligature to the Arabic Unicode letter.

| Sequence |        Diacritic        |
| :------: | :---------------------: |
|   `''`   |  &#x064B;  *fathatan*   |
|   `'`    |    &#x064E;  *fatha*    |
|   `--`   |  &#x064D;  *kasratan*   |
|   `-`    |    &#x0650;  *kasra*    |
|   `%%`   |  &#x064C;  *dammatan*   |
|   `%`    |    &#x064F;  *damma*    |
|   `$`    |   &#x0651;  *shadda*    |
|   `#`    |    &#x0652;  *sukun*    |
|   `@`    | &#x654;  *hamza above*  |
|   `.@`   | &#x655;  *hamza below*  |
|   `~`    | &#x653;  *maddah above* |

Note that diacritics also act as delimiters between letters. For example:

```
> alif.@-noon$'alif lam-lam$'haa- waw'alif.@-noon$'alif alif.@-lam'ya#haa- ra'alif_jeem-ayn%waw_noon'
```

Will copy this text:

> إِنَّا لِلَّهِ وَإِنَّا إِلَيْهِ رَاجِعُونَ

### How to Use Macros

Use `add inna alif.@-noon$'alif` to add إِنَّا in place of `inna` when Arabizing text.

Use `add-lit ArRahman الرَّحْمَان` to add الرَّحْمَان using its literal Unicode characters in place of `ArRahman`.

Use `remove inna` to remove the macro `inna`.

## Arguments

- `help` or `h`: Display arguments
- `letters` or `l`: List all transliterated letters and their Unicode mappings
- `diacritics` or `d`: List all transliterated diacritics and their Unicode mappings
- `macros` or `m`: List all macros with their key-value mappings
- `open` or `o`: Open the settings JSON file.
- `open macros`: Open the macros JSON file.
- `add [key] [value]` or `a [key] [value]`: Add a new macro where the value is Arabized
- `add-lit [key] [value]` or `al [key] [value]`: Add a new macro where the value is literal
- `remove [key]` or `rm [key]`: Remove an existing macro
- `clear` or `c`: Clear the console screen
- `quit` or `q`: Exit the program

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/Arabize.git`
2. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
3. Build the application using the .NET CLI: `dotnet msbuild`
4. Run the executable located in `Arabize/bin`

### Releasing

```
dotnet msbuild --property:Configuration=Release && cd Arabize/bin/Release && 7z a Arabize.zip * && gh release create v1.0.0 ./Arabize.zip -t "v1.0.0" --target main -F ./RELEASE.md && cd ../../..
```

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).
