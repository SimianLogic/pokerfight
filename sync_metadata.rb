#!/usr/bin/env ruby

require 'FileUtils'

def copy_and_move(root_name)
  metadata    = "documents/#{root_name}/metadata.txt"
  new_metadata = "documents/#{root_name}/#{root_name}.txt"
  
  FileUtils.cp(metadata, new_metadata)
  FileUtils.mv(new_metadata, "unity/Assets/Resources/Metadata")
end

metadata = Dir.glob("documents/**/metadata.txt")
metadata.each do |meta|
  root_name = meta.split("/")[1]  
  copy_and_move(root_name)
end  
