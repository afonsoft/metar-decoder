import os
import sys

# Configuration file for the Sphinx documentation builder.

# -- Path setup --------------------------------------------------------------

sys.path.insert(0, os.path.abspath('.'))


# -- Project information -----------------------------------------------------

project = 'metar-decoder'
copyright = '2024, AFONSOFT'
author = 'Afonso Dutra Nogueira Filho'

# The full version, including alpha/beta/rc tags
release = '1.0.0'


# -- General configuration ---------------------------------------------------

extensions = [
    'sphinx.ext.autodoc',
    'sphinx.ext.doctest',
    'sphinx.ext.intersphinx',
    'sphinx.ext.todo',
    'sphinx.ext.coverage',
    'sphinx.ext.mathjax',
    'sphinx.ext.ifconfig',
    'sphinx.ext.viewcode',
]

templates_path = ['_templates']

exclude_patterns = []


# -- Options for HTML output -------------------------------------------------

html_theme = 'alabaster'

html_static_path = ['_static']