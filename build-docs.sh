#!/bin/bash

cd docs;
make clean
make html FRAMEWORK=aspnetcore
