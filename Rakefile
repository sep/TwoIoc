require 'rubygems'
require 'bundler/setup'
require 'albacore'
require 'fileutils'
require 'jeweler/version_helper'
require 'git'
require 'nokogiri'

include FileUtils

task :default => [:build, :test]

def build task_name, *targets
  build_properties = { :configuration => 'Debug', :platform => 'Any CPU', :TrackFileAccess => :false }
  build_properties.merge!(targets.pop) if Hash === targets.last 
  msbuild task_name do |msb|
    msb.properties build_properties
    msb.targets targets
    msb.solution = 'src/TwoIoc.sln'
  end
end

def mspec_build task_name, *options
  options ||= []
  options << '--xml output/mspec.xml'
  mspec task_name do |mspec|
    mspec.command = 'lib/mspec/mspec-x86-clr4.exe'
    mspec.assemblies = ['src/TwoIoc.Tests/bin/Debug/TwoIoc.Tests.dll']
    mspec.html_output = 'output/mspec.html'
    mspec.options options.join(' ') 
  end
end 

task :build => ['build:debug']

namespace :build do
  desc "Build in debug mode"
  build :debug, 'Build'
  task :debug

  desc "Build in release mode"
  build :release, 'Clean', 'Build', :configuration => 'Release'
  task :release
end

desc "Builds, runs tests, transforms mspec output"
task :test => [:build, :clearoutput, 'test:all', 'test:transform_mspec']

desc "Builds, runs tests, transforms mspec output (for CI)"
task :test_ci => [:build, :clearoutput, 'test:ci', 'test:transform_mspec']

namespace :test do

  desc "Runs tests appropriate for a dev machine"
  mspec_build 'all'

  task :transform_mspec do
    sys('lib/msxsl.exe output/mspec.xml lib/mspec/mspec2junit.xsl -o output/mspec.junit.xml')
  end

end

desc "Builds release"
task :release => ['build:clear', 'build:release', 'test', 'tag'] do
  puts "Created #{version_helper.to_s}"
end

def version_helper
  @version_helper ||= Jeweler::VersionHelper.new(File.dirname(__FILE__))
end

def repo
  @repo ||= Git.open(File.dirname(__FILE__))
end

def commit opts
  repo.add opts[:files]
  repo.commit opts[:message]
end

desc 'Show the current version'
task :version do
  $stdout.puts "Current version: #{version_helper.to_s}"
end

namespace :version do
  task :cs do
    File.open('src/Version.cs', 'w') do |f|
      f.puts(
        'partial class TwoIocProperties',
        '{',
        "    public const string Version = \"#{version_helper.to_s}\";",
        '}'
      )
    end
    $stdout.puts 'Wrote Version.cs'
    Rake::Task['version:cs'].reenable
  end

  namespace :bump do
    desc "Bump to next major version"
    task :major => :version do
      version_helper.bump_major
      version_helper.write
      $stdout.puts "Updated version to #{version_helper.to_s}"
      Rake::Task['version:cs'].invoke
      commit(:files => ['VERSION', 'src/Version.cs'], :message => "Bumped to version #{version_helper.to_s}")
    end
    desc "Bump to next minor version"
    task :minor => :version do
      version_helper.bump_minor
      version_helper.write
      $stdout.puts "Updated version to #{version_helper.to_s}"
      Rake::Task['version:cs'].invoke
      commit(:files => ['VERSION', 'src/Version.cs'], :message => "Bumped to version #{version_helper.to_s}")
    end
    desc "Bump to next patch version"
    task :patch => :version do
      version_helper.bump_patch
      version_helper.write
      $stdout.puts "Updated version to #{version_helper.to_s}"
      Rake::Task['version:cs'].invoke
      commit(:files => ['VERSION', 'src/Version.cs'], :message => "Bumped to version #{version_helper.to_s}")
    end
  end
end

task :tag do
  release_tag = "v#{version_helper.to_s}"
  tag = repo.tag(release_tag) rescue nil
  unless tag
    $stdout.puts "Tagging #{release_tag}"
    repo.add_tag(release_tag)
  end
end

task :clearoutput do
  rm_rf 'output'
  mkdir 'output'
end

def sys(cmd)
  puts "[#{cmd}]"
  result = `#{cmd}`
  exit 1 unless result
end
