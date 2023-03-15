# Arabize

This is a command to translate Arabic-transliterated letters into Arabic Unicode characters and copy to clipboard.

To use it, place the path of the `Arabize` directory in `Path` for System Variables.

To recompile, use `csc arabize-script.c`.

## How to use:

Pass the transliterated letters to have the Arabic Unicode copied to clipboard.

```
$ arabize alif_lam_ra_ha_meem_alif_noon
```

This copies this text:

> الرحمان 

Use `_` characters to delimit letters. Use space characters to delimit words.

```
$ arabize lam_alif alif_lam_haa alif_lam_alif alif_lam_lam_haa
```

This copies this text:

> لا اله الا الله 

## How to Use Macros:

Use `arabize add laa lam_alim` to add لا in place of `laa`.

Use `arabize add-lit laa لا ` to add لا using its literal Unicode characters in place of `laa`.

Use `arabize remove laa` to remove the macro.
