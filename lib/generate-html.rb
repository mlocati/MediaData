require "asciidoctor"

adoc = Asciidoctor.convert_file "../index.adoc", :to_file => false, :header_footer => true, :safe => Asciidoctor::SafeMode::UNSAFE, :attributes => {"numbered" => true}

f = File.open("../index.html", "wb")
f.write(adoc)
f.close
