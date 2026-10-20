# Repository security

On 2026-10-20, the Git history was rewritten to remove exposed login credentials,
a revoked Dropbox token, and a previously committed IPA containing credentials.

If you cloned this repository before that cleanup, clone it again into a new
directory. Do not merge or push branches from the old clone: doing so can restore
the exposed history. Reapply any unpublished changes to the new clone after
checking them for credentials.

Keep passwords and access tokens out of source code. Publish IPA, APK, and AAB
files as release assets instead of committing them. The Secret scan workflow
checks Git history, including short passwords that generic detectors can miss.

Firebase client configuration is public by design. Restrict its keys to the
required APIs and apps, and protect backend data with Firebase Security Rules.
