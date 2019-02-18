require 'rake'
require 'rake/clean'
require 'fileutils'

# --------------------------------------------------------------------------- #
# Configuration
# --------------------------------------------------------------------------- #
SOLUTION       = 'Cube.FileSystem.SevenZip.Ice'
PACKAGE        = 'Cube.FileSystem.SevenZip'
NATIVE         = '../resources/native'
BRANCHES       = [ 'master', 'net35' ]
PLATFORMS      = [ 'x86', 'x64' ]
CONFIGURATIONS = [ 'Debug', 'Release' ]
TESTCASES      = {
    'Cube.FileSystem.SevenZip.Tests'     => 'Tests',
    'Cube.FileSystem.SevenZip.Ice.Tests' => 'Applications/Ice/Tests'
}

# --------------------------------------------------------------------------- #
# Commands
# --------------------------------------------------------------------------- #
CHECKOUT = 'git checkout'
BUILD    = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
RESTORE  = 'nuget restore'
PACK     = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'
TEST     = '../packages/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe'

# --------------------------------------------------------------------------- #
# Functions
# --------------------------------------------------------------------------- #
def do_copy(src, dest)
    FileUtils.mkdir_p(dest)
    FileUtils.cp_r(src, dest)
end

# --------------------------------------------------------------------------- #
# Tasks
# --------------------------------------------------------------------------- #
task :default do
    Rake::Task[:clean].execute
    Rake::Task[:build].execute
    Rake::Task[:copy].execute
    Rake::Task[:pack].execute
end

# --------------------------------------------------------------------------- #
# Build
# --------------------------------------------------------------------------- #
task :build do
    BRANCHES.each { |branch|
        sh("#{CHECKOUT} #{branch}")
        sh("#{RESTORE} #{SOLUTION}.sln")
        sh("#{BUILD} #{SOLUTION}.sln")
    }
end

# --------------------------------------------------------------------------- #
# Pack
# --------------------------------------------------------------------------- #
task :pack do
    sh("#{CHECKOUT} net35")
    sh("#{PACK} Libraries/#{PACKAGE}.nuspec")
    sh("#{CHECKOUT} master")
end

# --------------------------------------------------------------------------- #
# Copy
# --------------------------------------------------------------------------- #
task :copy do
    [ '', 'net35' ].product(PLATFORMS, CONFIGURATIONS) { |set|
        x86_64  = [ 'bin', set[0], set[1], set[2] ].compact.reject(&:empty?).join('/')
        any_cpu = [ 'bin', set[0], set[2] ].compact.reject(&:empty?).join('/')

        [ 'Tests', 'Applications/Ice/Tests', 'Applications/Ice/Progress' ].each { |dest|
            dir = [ NATIVE, set[1] ].join('/')
            src = Dir.glob("#{dir}/7z/7z.*")
            do_copy(src, "#{dest}/#{x86_64}")
            do_copy(src, "#{dest}/#{any_cpu}") if (set[1] == 'x64')
        }
    }
end

# --------------------------------------------------------------------------- #
# Test
# --------------------------------------------------------------------------- #
task :test do
    sh("#{RESTORE} #{SOLUTION}.sln")
    sh("#{BUILD} #{SOLUTION}.sln")

    branch = `git symbolic-ref --short HEAD`.chomp
    TESTCASES.each { |proj, dir|
        src = branch == 'net35' ?
              "#{dir}/bin/net35/Release/#{proj}.dll" :
              "#{dir}/bin/Release/#{proj}.dll"
        sh("#{TEST} #{src}")
    }
end

# --------------------------------------------------------------------------- #
# Clean
# --------------------------------------------------------------------------- #
CLEAN.include("#{PACKAGE}.*.nupkg")
CLEAN.include(%w{dll log}.map{ |e| "**/*.#{e}" })