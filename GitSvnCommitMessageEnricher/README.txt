This little application appends the following text to the commit message used by 'git svn dcommit':

-------
Committed by git-svn. Additional info about the commit:
Author: John Doe <john.doe@acme.com> @ 12.11.2011 21:22:04 +01:00
Committer: John Doe <john.doe@acme.com> @ 12.11.2011 21:22:04 +01:00


To use it, it is best to create an alias:

git config --global alias.svn-commit '!GIT_EDITOR="C:/Program Files/Git/GitSvnCommitMessageEnricher.exe" git svn dcommit --edit'

Then use "git svn-commit" instead of "git svn dcommit".

Thanks to Dan Cruz: http://stackoverflow.com/questions/3020827/git-to-svn-adding-commit-date-to-log-messages/8097550#8097550

Important:
* If you want to change the text that is appended, be sure to only include UNIX line endings, otherwise SVN will not accept the commit.
* Be aware that parenthesis are not supported in the path to the GIT_EDITOR, for example on a x64 machine where you might want to put the file into "Program Files (x86)". This is not going to work, unfortunately.
* The additional info will also be visible in your GIT repository.