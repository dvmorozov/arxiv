# -*- coding: utf-8 -*-
import os
import re
import fnmatch
import argparse
from textwrap import dedent


parser = argparse.ArgumentParser(description='Add/update copyright on C# files')
parser.add_argument('root', nargs=1, help='Path to the root of the C# project')
args = parser.parse_args()

# Descend into the 'root' directory and find all *.cs files
files = []
for root, dirnames, filenames in os.walk(args.root[0]):
    for filename in fnmatch.filter(filenames, "*.cs"):
		files.append(os.path.join(root, filename))
print "Found {0} *.cs files".format(len(files))

for filepath in files:
    with open(filepath) as f:
        contents = f.read()

    # This regex will separate the contents of a *.cs file into two parts.
    # The first part is any text that appears before either 'using' or
    # 'namespace' - perhaps an old copyright. The second part *should* be
    # the actual code beginning with 'using' or 'namespace'.
    match = re.search(r"^.*?((using|namespace|/\*|#).+)$", contents, re.DOTALL)
    if match:
        # Make the file's now contain the user defined copyright (below)
        # followed by a blank line followed by the actual code.
        contents = dedent('''\
            // ****************************************************************************
            //    File "{0}"
            //    Copyright © Dmitry Morozov 2022
            //    If you want to use this file please contact me by dvmorozov@hotmail.com.
            // ****************************************************************************
    
            ''').format(os.path.basename(filepath)) + match.group(1)
        with open(filepath, 'w') as f:
            f.write(contents)
    	print "Wrote new: {0}".format(filepath)